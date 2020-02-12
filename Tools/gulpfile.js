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


gulp.task('clean-output', function (cb) {
  return del([
    '../Functions.Templates/bin/Portal/out'
  ], { force: true });
});

gulp.task('resources-clean', function (cb) {
  return del([
    '../template-downloads',
    '../Functions.Templates/bin/Portal/3',
    '../Functions.Templates/bin/Portal/3-convert',
  ], { force: true });
});

/*****
 * Download and unzip nuget packages with templates
 */
gulp.task('download-templates', function () {
  return gulp.src('../Functions.Templates/bin/Portal/Nuget/*.nupkg')
    .pipe(gulp.dest('../template-downloads/3/'));
});

gulp.task('unzip-templates', function () {
  let streams = [];
  streams.push(
    gulp
      .src(`../template-downloads/3/*`)
      .pipe(decompress())
      .pipe(gulp.dest(`../Functions.Templates/bin/Portal/3`))
  );
  return gulpMerge(streams);
});

/********
 *   This task takes the Resource Resx files from both templates folder and Portal Resources Folder and converts them to json, it drops them into a intermediate 'convert' folder.
 *   Also it will change the file name format to Resources.<language code>.json
 */

gulp.task('resources-convert', function () {
  return gulp
    .src(['../Functions.Templates/bin/Portal/3/Resources/**/Resources.resx'])
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
    .pipe(gulp.dest('../Functions.Templates/bin/Portal/3-convert/resources-convert'));
});

/********
 *   This is the task takes the output of the convert task and formats the json to be in the format that gets sent back to the client by the API, it's easier to do this here than at the end
 */
gulp.task('resources-build', function () {
  const streams = [];

  streams.push(
    gulp
      .src(['../Functions.Templates/bin/Portal/3-convert/resources-convert/**/Resources.*.json'])
      .pipe(
        jeditor(function (json) {
          const enver = require(path.normalize('../Functions.Templates/bin/Portal/3-convert/resources-convert/Resources.json'));
          const retVal = {
            lang: json,
            en: enver,
          };

          return retVal;
        })
      )
      .pipe(gulp.dest('../Functions.Templates/bin/Portal/out/resources'))
  );

  streams.push(
    gulp
      .src(['../Functions.Templates/bin/Portal/3-convert/resources-convert/Resources.json'])
      .pipe(
        jeditor(function (json) {
          const retVal = {
            en: json,
          };

          return retVal;
        })
      )
      .pipe(gulp.dest('../Functions.Templates/bin/Portal/out/resources'))
  );

  return gulpMerge(streams);
});

gulp.task('resources-copy', function () {
  return gulp.src('../Functions.Templates/bin/Portal/out/resources/Resources.json')
    .pipe(rename('Resources.en-US.json'))
    .pipe(gulp.dest('../Functions.Templates/bin/Portal/out/resources'));
});

/***********************************************************
 * Templates Building
 */

gulp.task('build-templates', function (cb) {
  const version = '3';
  let templateListJson = [];
  const templates = getSubDirectories(path.join('../Functions.Templates/bin/Portal/3', 'Templates'));
  templates.forEach(template => {
    let templateObj = {};
    const filePath = path.join('../Functions.Templates/bin/Portal/3', 'Templates', template);
    let files = getFilesWithContent(filePath, ['function.json', 'metadata.json']);

    templateObj.id = template;
    templateObj.runtime = version;
    templateObj.files = files;

    templateObj.function = require(path.join(filePath, 'function.json'));
    templateObj.metadata = require(path.join(filePath, 'metadata.json'));
    templateListJson.push(templateObj);
  });
  let writePath = path.join('../Functions.Templates/bin/Portal/out', 'templates');
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
  const bindingFile = require(path.join('../Functions.Templates/bin/Portal/3', 'Bindings', 'bindings.json'));
  bindingFile.bindings.forEach(binding => {
    if (binding.documentation) {
      const documentationSplit = binding.documentation.split('\\');
      const documentationFile = documentationSplit[documentationSplit.length - 1];
      const documentationString = fs.readFileSync(path.join('../Functions.Templates/bin/Portal/3', 'Documentation', documentationFile), {
        encoding: 'utf8',
      });
      binding.documentation = documentationString;
    }
  });
  let writePath = path.join('../Functions.Templates/bin/Portal/out', 'bindings');
  if (!fs.existsSync(writePath)) {
    fs.mkdirSync(writePath);
  }
  writePath = path.join(writePath, 'bindings.json');
  fs.writeFileSync(writePath, new Buffer(JSON.stringify(bindingFile, null, 2)));
  cb();
});

gulp.task('zip-output', function () {
  return gulp.src('../Functions.Templates/bin/Portal/out/**/*.json')
    .pipe(zip('out.zip'))
    .pipe(gulp.dest('../Functions.Templates/bin/Portal'));
});

gulp.task(
  'build-all',
  gulp.series(
    'clean-output',
    'resources-clean',
    'download-templates',
    'unzip-templates',
    'resources-convert',
    'resources-build',
    'resources-copy',
    'build-templates',
    'build-bindings',
    'zip-output',
    'resources-clean'
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