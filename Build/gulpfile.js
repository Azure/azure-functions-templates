const gulp = require('gulp');
const resx2 = require('./gulp-utils/gulp-resx-js');
const rename = require('gulp-rename');
const gulpMerge = require('merge-stream');
const jeditor = require('gulp-json-editor');
const fs = require('fs');
const path = require('path');
const del = require('del');
const decompress = require('gulp-decompress');
const zip = require('gulp-zip');
const request = require('request');
const nuget = require('gulp-nuget');

buildVersion = '1';

if (process.env.devops_buildNumber) {
  buildVersion = process.env.devops_buildNumber;
}

gulp.task('nuget-pack', function () {
  let streams = [];
  var nugetPath = './nuget.exe';
  var patchVersion = 'patchVersion=' + buildVersion

  let files = getFiles('./PackageFiles/Dotnet_precompiled');
  for (let i = 0; i < files.length; i++) {
    let filePath = './PackageFiles/Dotnet_precompiled/' + files[i];
    streams.push(
      gulp
        .src(filePath)
        .pipe(nuget.pack({ nuget: nugetPath, properties: patchVersion, outputDirectory: '../bin/VS/' }))
    );
  }

  files = getFiles('./PackageFiles/ExtensionBundle');
  for (let i = 0; i < files.length; i++) {
    let filePath = './PackageFiles/ExtensionBundle/' + files[i];
    streams.push(
      gulp
        .src(filePath)
        .pipe(nuget.pack({ nuget: nugetPath, properties: patchVersion, outputDirectory: "../bin/Temp/ExtensionBundle" }))
    )
  }

  return gulpMerge(streams);
});

gulp.copy = function (src, dest) {
  return gulp.src(src).pipe(gulp.dest(dest));
};

gulp.task('nuget-download', function (done) {
  if (fs.existsSync('nuget.exe')) {
    return done();
  }

  request.get('https://dist.nuget.org/win-x86-commandline/v6.0.0/nuget.exe')
    .pipe(fs.createWriteStream('nuget.exe'))
    .on('close', done);
});

gulp.task('clean-output', function (cb) {
  return del([
    '../../Functions.Templates/bin/Portal/out'
  ], { force: true });
});

gulp.task('clean', function (cb) {
  return del([
    '../bin'
  ], { force: true });
});

gulp.task('clean-temp', function (cb) {
  return del([
    '../bin/Temp'
  ], { force: true });
});

gulp.task('unzip-templates', function () {
  let streams = [];

  let files = getFiles('../bin/Temp/ExtensionBundle');
  for (let i = 0; i < files.length; i++) {
    let filePath = '../bin/Temp/Temp-' + files[i].replace(".nupkg", "")

    streams.push(
      gulp
        .src(path.join('../bin/Temp/ExtensionBundle', files[i]))
        .pipe(decompress())
        .pipe(gulp.dest(filePath))
    );
  }

  return gulpMerge(streams);
});

/********
 *   This task takes the Resource Resx files from both templates folder and Portal Resources Folder and converts them to json, it drops them into a intermediate 'convert' folder.
 *   Also it will change the file name format to Resources.<language code>.json
 */

gulp.task('resources-convert', function () {
  const streams = [];

  let files = getFiles('../bin/Temp/ExtensionBundle');
  for (let i = 0; i < files.length; i++) {
    let fileName = files[i].replace(".nupkg", "");
    let dirPath = path.join('../bin/Temp/', 'Temp-' + fileName);
    let resourceFile = path.join(dirPath, 'Resources') + '/**/Resources.resx';
    let resourceFileSimple = path.join(dirPath, 'Resources', 'Resources.resx');
    let convertPath = path.join(dirPath, 'resources-convert')

    if (!fs.existsSync(resourceFileSimple)) {
      continue;
    }

    streams.push(
      gulp.src([resourceFile])
        .pipe(resx2())
        .pipe(rename(function (p) {
          const language = p.dirname.split(path.sep)[0];
          if (!!language && language !== '.') {
            p.basename = 'Resources.' + language;
          }
          p.dirname = '.';
          p.extname = '.json';
        }))
        .pipe(gulp.dest(convertPath)));
  }
  return gulpMerge(streams);
});

/********
 *   This is the task takes the output of the convert task and formats the json to be in the format that gets sent back to the client by the API, it's easier to do this here than at the end
 */
gulp.task('resources-build', function () {
  const streams = [];
  let files = getFiles('../bin/Temp/ExtensionBundle');
  for (let i = 0; i < files.length; i++) {
    let fileName = files[i].replace(".nupkg", "");
    let dirPath = path.join('../bin/Temp/', 'Temp-' + fileName);
    let resourceFileSimple = path.join(dirPath, 'Resources', 'Resources.resx');

    if (!fs.existsSync(resourceFileSimple)) {
      continue;
    }

    streams.push(
      gulp
        .src(['../bin/Temp/Temp-' + fileName + '/resources-convert/**/Resources.*.json'])
        .pipe(
          jeditor(function (json) {
            const enver = require('../bin/Temp/Temp-' + fileName + '/resources-convert/Resources.json');
            const retVal = {
              lang: json,
              en: enver,
            };

            return retVal;
          })
        )
        .pipe(gulp.dest('../bin/Temp/out/' + fileName + '/resources'))
    );

    streams.push(
      gulp
        .src(['../bin/Temp/Temp-' + fileName + '/resources-convert/Resources.json'])
        .pipe(
          jeditor(function (json) {
            const retVal = {
              en: json,
            };

            return retVal;
          })
        )
        .pipe(gulp.dest('../bin/Temp/out/' + fileName + '/resources'))
    );

    streams.push(
      gulp
        .src(['../bin/Temp/Temp-' + fileName + '/resources-convert/**/Resources.*.json'])
        .pipe(
          jeditor(function (json) {
            const enver = require('../bin/Temp/Temp-' + fileName + '/resources-convert/Resources.json');
            const retVal = {
              lang: json,
              en: enver,
            };

            return retVal;
          })
        )
        .pipe(gulp.dest('../bin/Temp/out/' + fileName + '/resources-v2'))
    );

    streams.push(
      gulp
        .src(['../bin/Temp/Temp-' + fileName + '/resources-convert/Resources.json'])
        .pipe(
          jeditor(function (json) {
            const retVal = {
              en: json,
            };

            return retVal;
          })
        )
        .pipe(gulp.dest('../bin/Temp/out/' + fileName + '/resources-v2'))
    );
  }
  return gulpMerge(streams);
});

gulp.task('resources-copy', function () {
  const streams = [];
  let files = getFiles('../bin/Temp/ExtensionBundle');
  for (let i = 0; i < files.length; i++) {
    let fileName = files[i].replace(".nupkg", "");
    let dirPath = path.join('../bin/Temp/', 'Temp-' + fileName);
    let resourceFileSimple = path.join(dirPath, 'Resources', 'Resources.resx');

    if (!fs.existsSync(resourceFileSimple)) {
      continue;
    }

    streams.push(
      gulp.src('../bin/Temp/out/' + fileName + '/resources/Resources.json')
        .pipe(rename('Resources.en-US.json'))
        .pipe(gulp.dest('../bin/Temp/out/' + fileName + '/resources'))
    );

    streams.push(
      gulp.src('../bin/Temp/out/' + fileName + '/resources/Resources.json')
        .pipe(rename('Resources.en-US.json'))
        .pipe(gulp.dest('../bin/Temp/out/' + fileName + '/resources-v2'))
    );
  }
  return gulpMerge(streams);
});

gulp.task('userprompt-copy', function () {
  const streams = [];
  let files = getFiles('../bin/Temp/ExtensionBundle');
  for (let i = 0; i < files.length; i++) {
    let fileName = files[i].replace(".nupkg", "");
    let dirPath = path.join('../bin/Temp/', 'Temp-' + fileName);
    let userpromptSimple = path.join(dirPath, 'Bindings-v2', 'userPrompts.json');

    if (!fs.existsSync(userpromptSimple)) {
      continue;
    }

    streams.push(
      gulp.src(userpromptSimple)
        .pipe(rename('userPrompts.json'))
        .pipe(gulp.dest('../bin/Temp/out/' + fileName + '/bindings-v2'))
    );
  }
  return gulpMerge(streams);
});

/***********************************************************
 * Templates Building
 */

gulp.task('build-templates', function (cb) {

  let files = getFiles('../bin/Temp/ExtensionBundle');
  for (let i = 0; i < files.length; i++) {
    let fileName = files[i].replace(".nupkg", "");
    let dirPath = '../bin/Temp/Temp-' + fileName
    let templatesDir = path.join(dirPath, 'templates');

    if (!fs.existsSync(templatesDir)) {
      continue;
    }

    const version = '2';
    let templateListJson = [];
    const templates = getSubDirectories(path.join(dirPath, 'templates'));
    templates.forEach(template => {
      let templateObj = {};
      const filePath = path.join(dirPath, 'templates', template);
      let files = getFilesWithContent(filePath, ['function.json', 'metadata.json']);

      templateObj.id = template;
      templateObj.runtime = version;
      templateObj.files = files;

      templateObj.function = require(path.join(filePath, 'function.json'));
      templateObj.metadata = require(path.join(filePath, 'metadata.json'));
      templateListJson.push(templateObj);
    });

    let writeSubPath = path.join('../bin/Temp/', fileName);
    let writePath = path.join('../bin/Temp/out', fileName, 'templates');

    if (!fs.existsSync(writeSubPath)) {
      fs.mkdirSync(writeSubPath);
    }

    if (!fs.existsSync(writePath)) {
      fs.mkdirSync(writePath);
    }
    writePath = path.join(writePath, 'templates.json');
    fs.writeFileSync(writePath, new Buffer(JSON.stringify(templateListJson, null, 2)));
    cb();

  }
});

gulp.task('build-templates-v2', function (cb) {

  let files = getFiles('../bin/Temp/ExtensionBundle');
  for (let i = 0; i < files.length; i++) {
    let fileName = files[i].replace(".nupkg", "");
    let dirPath = '../bin/Temp/Temp-' + fileName
    let templatesDir = path.join(dirPath, 'templates-v2');

    if (!fs.existsSync(templatesDir)) {
      continue;
    }

    let templateListJson = [];
    const templates = getSubDirectories(path.join(dirPath, 'templates-v2'));
    templates.forEach(template => {
      let templateObj = {};
      const filePath = path.join(dirPath, 'templates-v2', template);
      let files = getFilesWithContent(filePath, ['template.json']);
      templateObj = require(path.join(filePath, 'template.json'));
      templateObj.id = template;
      templateObj.files = files;
      templateListJson.push(templateObj);
    });

    let writeSubPath = path.join('../bin/Temp/', fileName);
    let writePath = path.join('../bin/Temp/out', fileName, 'templates-v2');

    if (!fs.existsSync(writeSubPath)) {
      fs.mkdirSync(writeSubPath);
    }

    if (!fs.existsSync(writePath)) {
      fs.mkdirSync(writePath);
    }
    writePath = path.join(writePath, 'templates.json');
    fs.writeFileSync(writePath, new Buffer(JSON.stringify(templateListJson, null, 2)));
    cb();

  }
});

/********
 * Place Binding Templates
 */

gulp.task('build-bindings', function (cb) {
  let files = getFiles('../bin/Temp/ExtensionBundle');
  for (let i = 0; i < files.length; i++) {
    let fileName = files[i].replace(".nupkg", "");
    let dirPath = '../bin/Temp/Temp-' + fileName
    const bindings = path.join(dirPath, 'Bindings', 'bindings.json');

    if (fs.existsSync(bindings)) {
      const bindingFile = require(path.join(dirPath, 'Bindings', 'bindings.json'));
      bindingFile.bindings.forEach(binding => {
        if (binding.documentation) {
          const documentationSplit = binding.documentation.split('\\');
          const documentationFile = documentationSplit[documentationSplit.length - 1];
          const documentationString = fs.readFileSync(path.join('../Functions.Templates/', 'Documentation', documentationFile), {
            encoding: 'utf8',
          });
          binding.documentation = documentationString;
        }
      });

      let arifactPath = '../bin/Temp/out/' + fileName
      let writePath = path.join(arifactPath, 'bindings');

      if (!fs.existsSync(arifactPath)) {
        fs.mkdirSync(arifactPath);
      }

      if (!fs.existsSync(writePath)) {
        fs.mkdirSync(writePath);;
      }
      writePath = path.join(writePath, 'bindings.json');
      fs.writeFileSync(writePath, new Buffer(JSON.stringify(bindingFile, null, 2)));
    }
  }
  cb();
});

gulp.task('zip-output', function () {
  let dirs = getSubDirectories("../bin/Temp/out")
  let streams = [];

  for (let i = 0; i < dirs.length; i++) {
    streams.push(
      gulp.src('../bin/Temp/out/' + dirs[i] + '/**/*.json')
        .pipe(zip(dirs[i] + '.zip'))
        .pipe(gulp.dest('../bin/'))
    );
  }
  return gulpMerge(streams);
});

gulp.task(
  'build-all',
  gulp.series(
    'clean',
    'nuget-download',
    'nuget-pack',
    'unzip-templates',
    'resources-convert',
    'resources-build',
    'resources-copy',
    'userprompt-copy',
    'build-templates',
    'build-templates-v2',
    'build-bindings',
    'zip-output',
    'clean-temp'
  )
);

/********
 * UTILITIES
 */
function makeStreams() {
  files.forEach(function (file) {
    let thisParentFolders = path.dirname(file).substr(file.indexOf(path.sep));

    if (parentFolders.indexOf(thisParentFolders) === -1) {
      parentFolders.push(thisParentFolders);
    }
  });

  parentFolders.forEach(function (folder) {
    let foldersFile = folder.substr(folder.indexOf(path.sep));

    baseNames.forEach(function (baseName) {
      streams.push(
        files.filter(function (file) {
          return file.endsWith(path.join(foldersFile, baseName));
        })
      );
    });
  });
  streams = streams.filter(stream => stream.length >= 1);
}

function getSubDirectories(folder) {
  if (!fs.existsSync(folder)) {
    return [];
  }
  const dir = p => fs.readdirSync(p).filter(f => fs.statSync(path.join(p, f)).isDirectory());
  return dir(folder);
}

function getFilesWithContent(folder, filesToIgnore) {
  if (!fs.existsSync(folder)) {
    return {};
  }
  let obj = {};
  const fileNames = fs.readdirSync(folder).filter(f => fs.statSync(path.join(folder, f)).isFile());
  fileNames
    .filter(x => filesToIgnore.indexOf(x) === -1)
    .forEach(fileName => {
      const fileContent = fs.readFileSync(path.join(folder, fileName), {
        encoding: 'utf8',
      });
      obj[fileName] = fileContent;
    });

  return obj;
}

function newGuid() {
  return 'xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
    var r = (Math.random() * 16) | 0,
      v = c == 'x' ? r : (r & 0x3) | 0x8;
    return v.toString(16);
  });
}

function getFiles(folder) {
  if (!fs.existsSync(folder)) {
    return {};
  }
  let obj = {};
  return fileNames = fs.readdirSync(folder).filter(f => fs.statSync(path.join(folder, f)).isFile());
}