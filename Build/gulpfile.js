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

if (process.env.devops_buildNumber) {
  version = '2.1.' + process.env.devops_buildNumber;
}
else {
  version = '2.0.0';
}

if (process.env.devops_buildNumber) {
  bundleTemplateVersion = '1.0.' + process.env.devops_buildNumber;
}
else {
  bundleTemplateVersion = '1.0.0';
}

if (process.env.devops_buildNumber) {
  bundleTemplateVersionV2 = '2.0.' + process.env.devops_buildNumber;
}
else {
  bundleTemplateVersionV2 = '2.0.0';
}

if (process.env.devops_buildNumber) {
  bundleTemplateVersionV3 = '3.0.' + process.env.devops_buildNumber;
}
else {
  bundleTemplateVersionV3 = '3.0.0';
}

gulp.copy = function (src, dest) {
  return gulp.src(src)
    .pipe(gulp.dest(dest));
};

gulp.task('nuget-pack-itemTemplate', function () {
  var nugetPath = './nuget.exe';

  return gulp
    .src('./PackageFiles/ItemTemplates.nuspec')
    .pipe(nuget.pack({ nuget: nugetPath, version: version }))
    .pipe(gulp.dest('../bin/VS/'));
});

gulp.task('nuget-pack-projectTemplates', function () {
  var nugetPath = './nuget.exe';

  return gulp.src('./PackageFiles/ProjectTemplates.nuspec')
    .pipe(nuget.pack({ nuget: nugetPath, version: version }))
    .pipe(gulp.dest('../bin/VS'));
});

gulp.task('nuget-pack-bundle-v1', function () {
  var nugetPath = './nuget.exe';

  return gulp.src('./PackageFiles/ExtensionBundleTemplates-1.x.nuspec')
    .pipe(nuget.pack({ nuget: nugetPath, version: version }))
    .pipe(gulp.dest('../bin/Temp-ExtensionBundle.Templates-v1'));
});

gulp.task('nuget-pack-bundle-v2', function () {
  var nugetPath = './nuget.exe';

  return gulp.src('./PackageFiles/ExtensionBundleTemplates-2.x.nuspec')
    .pipe(nuget.pack({ nuget: nugetPath, version: version }))
    .pipe(gulp.dest('../bin/Temp-ExtensionBundle.Templates-v2'));
});

gulp.task('nuget-pack-bundle-v3', function () {
  var nugetPath = './nuget.exe';

  return gulp.src('./PackageFiles/ExtensionBundleTemplates-3.x.nuspec')
    .pipe(nuget.pack({ nuget: nugetPath, version: version }))
    .pipe(gulp.dest('../bin/Temp-ExtensionBundle.Templates-v3'));
});

gulp.task('nuget-pack-Templates', function () {
  var nugetPath = './nuget.exe';

  return gulp
    .src('./PackageFiles/Templates.nuspec')
    .pipe(nuget.pack({ nuget: nugetPath, version: version }))
    .pipe(gulp.dest('../bin/Temp/'));
});

gulp.task('nuget-download', function (done) {
  if (fs.existsSync('nuget.exe')) {
    return done();
  }

  request.get('https://dist.nuget.org/win-x86-commandline/v4.1.0/nuget.exe')
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
    '../bin/Temp',
    '../bin/Temp-ExtensionBundle.Templates-v1',
    '../bin/Temp-ExtensionBundle.Templates-v2',
    '../bin/Temp-ExtensionBundle.Templates-v3'
  ], { force: true });
});

gulp.task('unzip-templates', function () {
  let streams = [];
  streams.push(
    gulp
      .src(`../bin/Temp/*`)
      .pipe(decompress())
      .pipe(gulp.dest(`../bin/Temp/`))
  );

  streams.push(
    gulp
      .src(`../bin/Temp-ExtensionBundle.Templates-v1/*`)
      .pipe(decompress())
      .pipe(gulp.dest(`../bin/Temp-ExtensionBundle.Templates-v1/`))
  );

  streams.push(
    gulp
      .src(`../bin/Temp-ExtensionBundle.Templates-v2/*`)
      .pipe(decompress())
      .pipe(gulp.dest(`../bin/Temp-ExtensionBundle.Templates-v2/`))
  );

  streams.push(
    gulp
      .src(`../bin/Temp-ExtensionBundle.Templates-v3/*`)
      .pipe(decompress())
      .pipe(gulp.dest(`../bin/Temp-ExtensionBundle.Templates-v3/`))
  );
  return gulpMerge(streams);
});

gulp.task('copy-bindings-resources-to-bundle', function () {
  let streams = [];
  streams.push(
    gulp.copy('../bin/Templates/bindings/*', '../bin/ExtensionBundle.Templates-v1/bindings/')
  );

  streams.push(
    gulp.copy('../bin/Templates/resources/*', '../bin/ExtensionBundle.Templates-v1/resources/')
  );

  streams.push(
    gulp.copy('../bin/Templates/bindings/*', '../bin/ExtensionBundle.Templates-v2/bindings/')
  );

  streams.push(
    gulp.copy('../bin/Templates/resources/*', '../bin/ExtensionBundle.Templates-v2/resources/')
  );

  streams.push(
    gulp.copy('../bin/Templates/bindings/*', '../bin/ExtensionBundle.Templates-v3/bindings/')
  );

  streams.push(
    gulp.copy('../bin/Templates/resources/*', '../bin/ExtensionBundle.Templates-v3/resources/')
  );
  

  return gulpMerge(streams);
});


/********
 *   This task takes the Resource Resx files from both templates folder and Portal Resources Folder and converts them to json, it drops them into a intermediate 'convert' folder.
 *   Also it will change the file name format to Resources.<language code>.json
 */

gulp.task('resources-convert', function () {
  return gulp
    .src(['../bin/Temp/Resources/**/Resources.resx'])
    .pipe(resx2())
    .pipe(
      rename(function (p) {
        const language = p.dirname.split(path.sep)[0];
        if (!!language && language !== '.') {
          p.basename = 'Resources.' + language;
        }
        p.dirname = '.';
        p.extname = '.json';
      })
    )
    .pipe(gulp.dest('../bin/Temp/resources-convert'));
});

/********
 *   This is the task takes the output of the convert task and formats the json to be in the format that gets sent back to the client by the API, it's easier to do this here than at the end
 */
gulp.task('resources-build', function () {
  const streams = [];

  streams.push(
    gulp
      .src(['../bin/Temp/resources-convert/**/Resources.*.json'])
      .pipe(
        jeditor(function (json) {
          const enver = require('../bin/Temp/resources-convert/Resources.json');
          const retVal = {
            lang: json,
            en: enver,
          };

          return retVal;
        })
      )
      .pipe(gulp.dest('../bin/Templates/resources'))
  );

  streams.push(
    gulp
      .src(['../bin/Temp/resources-convert/Resources.json'])
      .pipe(
        jeditor(function (json) {
          const retVal = {
            en: json,
          };

          return retVal;
        })
      )
      .pipe(gulp.dest('../bin/Templates/resources'))
  );

  return gulpMerge(streams);
});

gulp.task('resources-copy', function () {
  return gulp.src('../bin/Templates/resources/Resources.json')
    .pipe(rename('Resources.en-US.json'))
    .pipe(gulp.dest('../bin/Templates/resources'));
});

/***********************************************************
 * Templates Building
 */

gulp.task('build-templates', function (cb) {
  const version = '2';
  let templateListJson = [];
  const templates = getSubDirectories(path.join('../bin/Temp', 'templates'));
  templates.forEach(template => {
    let templateObj = {};
    const filePath = path.join('../bin/Temp', 'templates', template);
    let files = getFilesWithContent(filePath, ['function.json', 'metadata.json']);

    templateObj.id = template;
    templateObj.runtime = version;
    templateObj.files = files;

    templateObj.function = require(path.join(filePath, 'function.json'));
    templateObj.metadata = require(path.join(filePath, 'metadata.json'));
    templateListJson.push(templateObj);
  });
  let writePath = path.join('../bin/Templates', 'templates');
  if (!fs.existsSync(writePath)) {
    fs.mkdirSync(writePath);
  }
  writePath = path.join(writePath, 'templates.json');
  fs.writeFileSync(writePath, new Buffer(JSON.stringify(templateListJson, null, 2)));
  cb();
});

gulp.task('build-ExtensionBundle-v1-Templates', function (cb) {
  const version = '1';
  let templateListJson = [];
  const templates = getSubDirectories(path.join('../bin/Temp-ExtensionBundle.Templates-v1', 'templates'));
  templates.forEach(template => {
    let templateObj = {};
    const filePath = path.join('../bin/Temp-ExtensionBundle.Templates-v1', 'templates', template);
    let files = getFilesWithContent(filePath, ['function.json', 'metadata.json']);

    templateObj.id = template;
    templateObj.runtime = version;
    templateObj.files = files;

    templateObj.function = require(path.join(filePath, 'function.json'));
    templateObj.metadata = require(path.join(filePath, 'metadata.json'));
    templateListJson.push(templateObj);
  });
  let writePath = path.join('../bin/ExtensionBundle.Templates-v1', 'templates');
  if (!fs.existsSync(writePath)) {
    fs.mkdirSync(writePath);
  }
  writePath = path.join(writePath, 'templates.json');
  fs.writeFileSync(writePath, new Buffer(JSON.stringify(templateListJson, null, 2)));
  cb();
});

gulp.task('build-ExtensionBundle-v2-Templates', function (cb) {
  const version = '2';
  let templateListJson = [];
  const templates = getSubDirectories(path.join('../bin/Temp-ExtensionBundle.Templates-v2', 'templates'));
  templates.forEach(template => {
    let templateObj = {};
    const filePath = path.join('../bin/Temp-ExtensionBundle.Templates-v2', 'templates', template);
    let files = getFilesWithContent(filePath, ['function.json', 'metadata.json']);

    templateObj.id = template;
    templateObj.runtime = version;
    templateObj.files = files;

    templateObj.function = require(path.join(filePath, 'function.json'));
    templateObj.metadata = require(path.join(filePath, 'metadata.json'));
    templateListJson.push(templateObj);
  });
  let writePath = path.join('../bin/ExtensionBundle.Templates-v2', 'templates');
  if (!fs.existsSync(writePath)) {
    fs.mkdirSync(writePath);
  }
  writePath = path.join(writePath, 'templates.json');
  fs.writeFileSync(writePath, new Buffer(JSON.stringify(templateListJson, null, 2)));
  cb();
});

gulp.task('build-ExtensionBundle-v3-Templates', function (cb) {
  const version = '3';
  let templateListJson = [];
  const templates = getSubDirectories(path.join('../bin/Temp-ExtensionBundle.Templates-v3', 'templates'));
  templates.forEach(template => {
    let templateObj = {};
    const filePath = path.join('../bin/Temp-ExtensionBundle.Templates-v3', 'templates', template);
    let files = getFilesWithContent(filePath, ['function.json', 'metadata.json']);

    templateObj.id = template;
    templateObj.runtime = version;
    templateObj.files = files;

    templateObj.function = require(path.join(filePath, 'function.json'));
    templateObj.metadata = require(path.join(filePath, 'metadata.json'));
    templateListJson.push(templateObj);
  });
  let writePath = path.join('../bin/ExtensionBundle.Templates-v3', 'templates');
  if (!fs.existsSync(writePath)) {
    fs.mkdirSync(writePath);
  }
  writePath = path.join(writePath, 'templates.json');
  fs.writeFileSync(writePath, new Buffer(JSON.stringify(templateListJson, null, 2)));
  cb();
});

/********
 * Place Binding Templates
 */

gulp.task('build-bindings', function (cb) {
  const version = '3';
  const bindingFile = require(path.join('../bin/Temp', 'Bindings', 'bindings.json'));
  bindingFile.bindings.forEach(binding => {
    if (binding.documentation) {
      const documentationSplit = binding.documentation.split('\\');
      const documentationFile = documentationSplit[documentationSplit.length - 1];
      const documentationString = fs.readFileSync(path.join('../bin/Temp', 'Documentation', documentationFile), {
        encoding: 'utf8',
      });
      binding.documentation = documentationString;
    }
  });
  let writePath = path.join('../bin/Templates', 'bindings');
  if (!fs.existsSync(writePath)) {
    fs.mkdirSync(writePath);
  }
  writePath = path.join(writePath, 'bindings.json');
  fs.writeFileSync(writePath, new Buffer(JSON.stringify(bindingFile, null, 2)));
  cb();
});

gulp.task('zip-output', function () {
  let streams = [];
  streams.push(
    gulp.src('../bin/Templates/**/*.json')
      .pipe(zip(version + '.zip'))
      .pipe(gulp.dest('../bin/'))
  );

  streams.push(
    gulp.src('../bin/ExtensionBundle.Templates-v1/**/*.json')
      .pipe(zip('ExtensionBundle.v1.Templates.' + bundleTemplateVersion + '.zip'))
      .pipe(gulp.dest('../bin/'))
  );

  streams.push(
    gulp.src('../bin/ExtensionBundle.Templates-v2/**/*.json')
      .pipe(zip('ExtensionBundle.v2.Templates.' + bundleTemplateVersionV2 + '.zip'))
      .pipe(gulp.dest('../bin/'))
  );

  streams.push(
    gulp.src('../bin/ExtensionBundle.Templates-v3/**/*.json')
      .pipe(zip('ExtensionBundle.v3.Templates.' + bundleTemplateVersionV3 + '.zip'))
      .pipe(gulp.dest('../bin/'))
  );

  return gulpMerge(streams);
});

gulp.task(
  'build-all',
  gulp.series(
    'clean',
    'nuget-download',
    'nuget-pack-itemTemplate',
    'nuget-pack-projectTemplates',
    'nuget-pack-Templates',
    'nuget-pack-bundle-v1',
    'nuget-pack-bundle-v2',
    'nuget-pack-bundle-v3',
    'unzip-templates',
    'resources-convert',
    'resources-build',
    'resources-copy',
    'build-templates',
    'build-bindings',
    'copy-bindings-resources-to-bundle',
    'build-ExtensionBundle-v1-Templates',
    'build-ExtensionBundle-v2-Templates',
    'build-ExtensionBundle-v3-Templates',
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

function getFiles(folders) {
  let possibleDirectory;

  folders.forEach(function (folder, index) {
    let tempFiles = fs.readdirSync('./' + folder);

    tempFiles.forEach(function (fileOrDirectory) {
      possibleDirectory = path.join(folder, fileOrDirectory);
      if (fs.lstatSync(possibleDirectory).isDirectory()) {
        getFiles([possibleDirectory]);
      } else {
        files.push(path.join(folder, fileOrDirectory));

        if (baseNames.indexOf(fileOrDirectory) === -1) {
          baseNames.push(fileOrDirectory);
        }
      }
    });
  });
}