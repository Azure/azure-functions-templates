const gulp = require('gulp');
const resx2 = require('./gulp-utils/gulp-resx-js');
const rename = require('gulp-rename');
const gulpMerge = require('merge-stream');
const jeditor = require('gulp-json-editor');
const fs = require('fs');
const path = require('path');
const del = require('del');
const { exec } = require('child_process');
const zip = require('gulp-zip');
const https = require('https');
const nuget = require('gulp-nuget');
const { spawn, execSync } = require('child_process');

buildVersion = '1';

if (process.env.devops_buildNumber) {
  buildVersion = process.env.devops_buildNumber;
}

// Helper function to find available NuGet executable
function findNugetExecutable() {
  // On Windows, prefer local nuget.exe first
  if (process.platform === 'win32') {
    if (fs.existsSync('./nuget.exe')) {
      return './nuget.exe';
    }
  }
  
  // Check for system-installed nuget
  try {
    const command = process.platform === 'win32' ? 'where nuget' : 'which nuget';
    execSync(command, { stdio: 'ignore' });
    return 'nuget';
  } catch (e) {
    // nuget not found in PATH
  }
  
  // Check for dotnet (can be used as alternative)
  try {
    const command = process.platform === 'win32' ? 'where dotnet' : 'which dotnet';
    execSync(command, { stdio: 'ignore' });
    return 'dotnet';
  } catch (e) {
    // dotnet not found in PATH
  }
  
  return null;
}

// Helper function to check if NuGet packaging is available
function isNugetAvailable() {
  const nugetExe = findNugetExecutable();
  if (!nugetExe) {
    return false;
  }
  
  // For dotnet, we need to check if pack command is available
  if (nugetExe === 'dotnet') {
    try {
      execSync('dotnet pack --help', { stdio: 'ignore' });
      return true;
    } catch (e) {
      return false;
    }
  }
  
  return true;
}

gulp.task('nuget-pack', function (done) {
  // Check if NuGet is available
  if (!isNugetAvailable()) {
    console.log('NuGet not available on this system. Skipping nuget-pack task.');
    console.log('To enable NuGet packaging, install:');
    console.log('  - Windows: nuget.exe will be downloaded automatically');
    console.log('  - macOS: brew install nuget');
    console.log('  - Linux: Install .NET SDK (dotnet command)');
    return done();
  }

  let streams = [];
  var nugetPath = findNugetExecutable();
  var patchVersion = 'patchVersion=' + buildVersion

  console.log(`Using NuGet executable: ${nugetPath}`);

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

  if (streams.length === 0) {
    return done();
  }

  return gulpMerge(streams);
});

gulp.copy = function (src, dest) {
  return gulp.src(src).pipe(gulp.dest(dest));
};

gulp.task('nuget-download', function (done) {
  // Skip download on non-Windows platforms - rely on system nuget
  if (process.platform !== 'win32') {
    console.log('Skipping nuget.exe download on non-Windows platform');
    return done();
  }

  if (fs.existsSync('nuget.exe')) {
    return done();
  }

  const file = fs.createWriteStream('nuget.exe');
  https.get('https://dist.nuget.org/win-x86-commandline/v6.0.0/nuget.exe', (response) => {
    response.pipe(file);
    file.on('finish', () => {
      file.close(done);
    });
  }).on('error', (err) => {
    fs.unlink('nuget.exe', () => {}); // Delete the file on error
    done(err);
  });
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

gulp.task('unzip-templates', function (done) {
  let files = getFiles('../bin/Temp/ExtensionBundle');
  console.log('Found ExtensionBundle files:', files);
  
  if (files.length === 0) {
    console.log('No files to extract');
    return done();
  }

  let completedFiles = 0;
  const totalFiles = files.length;

  files.forEach((file) => {
    const sourceFile = path.resolve('../bin/Temp/ExtensionBundle', file);
    const targetDir = path.resolve('../bin/Temp/Temp-' + file.replace('.nupkg', ''));
    
    // Create target directory if it doesn't exist
    if (!fs.existsSync(targetDir)) {
      fs.mkdirSync(targetDir, { recursive: true });
    }
    
    console.log(`Extracting ${file} to ${targetDir}`);
    
    // Use system unzip command which is more reliable for nupkg files
    const unzipCmd = process.platform === 'win32' 
      ? `powershell -command "Expand-Archive -Path '${sourceFile}' -DestinationPath '${targetDir}' -Force"`
      : `unzip -q -o "${sourceFile}" -d "${targetDir}"`;
    
    exec(unzipCmd, (error, stdout, stderr) => {
      if (error) {
        console.error(`Error extracting ${file}:`, error.message);
      } else {
        console.log(`Successfully extracted ${file}`);
      }
      
      completedFiles++;
      if (completedFiles === totalFiles) {
        console.log('All extractions completed');
        done();
      }
    });
  });
});

/********
 *   This task takes the Resource Resx files from both templates folder and Portal Resources Folder and converts them to json, it drops them into a intermediate 'convert' folder.
 *   Also it will change the file name format to Resources.<language code>.json
 */

gulp.task('resources-convert', function (done) {
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
  
  if (streams.length === 0) {
    return done();
  }
  
  return gulpMerge(streams);
});

/********
 *   This is the task takes the output of the convert task and formats the json to be in the format that gets sent back to the client by the API, it's easier to do this here than at the end
 */
gulp.task('resources-build', function (done) {
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
  
  if (streams.length === 0) {
    return done();
  }
  
  return gulpMerge(streams);
});

gulp.task('resources-copy', function (done) {
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
      gulp.src('../bin/Temp/out/' + fileName + '/resources/Resources.json', { allowEmpty: true })
        .pipe(rename('Resources.en-US.json'))
        .pipe(gulp.dest('../bin/Temp/out/' + fileName + '/resources'))
    );

    streams.push(
      gulp.src('../bin/Temp/out/' + fileName + '/resources/Resources.json', { allowEmpty: true })
        .pipe(rename('Resources.en-US.json'))
        .pipe(gulp.dest('../bin/Temp/out/' + fileName + '/resources-v2'))
    );
  }
  
  if (streams.length === 0) {
    return done();
  }
  
  return gulpMerge(streams);
});

gulp.task('userprompt-copy', function (done) {
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
  
  if (streams.length === 0) {
    return done();
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
    fs.writeFileSync(writePath, Buffer.from(JSON.stringify(templateListJson, null, 2)));

  }
  cb();
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
    fs.writeFileSync(writePath, Buffer.from(JSON.stringify(templateListJson, null, 2)));

  }
  cb();
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
        fs.mkdirSync(writePath);
      }
      writePath = path.join(writePath, 'bindings.json');
      fs.writeFileSync(writePath, Buffer.from(JSON.stringify(bindingFile, null, 2)));
    }
  }
  cb();
});

gulp.task('zip-output', function (done) {
  let dirs = getSubDirectories("../bin/Temp/out")
  let streams = [];

  for (let i = 0; i < dirs.length; i++) {
    streams.push(
      gulp.src('../bin/Temp/out/' + dirs[i] + '/**/*.json')
        .pipe(zip(dirs[i] + '.zip'))
        .pipe(gulp.dest('../bin/'))
    );
  }
  
  if (streams.length === 0) {
    return done();
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

function getFiles(folder) {
  if (!fs.existsSync(folder)) {
    return [];
  }
  return fs.readdirSync(folder).filter(f => fs.statSync(path.join(folder, f)).isFile());
}