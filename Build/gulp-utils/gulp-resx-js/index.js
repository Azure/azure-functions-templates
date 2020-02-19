const resx2js = require('resx/resx2js');
var Buffer = require('buffer').Buffer;
var through2 = require('through2');
module.exports = function(opt) {
  opt = opt || {};

  // Convert XML to JSON
  var doConvert = async function(file) {
    var xml = file.contents.toString('utf8');
    return new Promise((resolve, reject) => {
      resx2js(xml, (err, res) => {
        if (!err) {
          resolve(JSON.stringify(res));
        } else {
          reject(err);
        }
      });
    });
  };

  var throughCallback = function(file, enc, cb) {
    if (file.isStream()) {
      return cb();
    }

    if (file.isBuffer()) {
      return doConvert(file).then(json => {
        file.contents = new Buffer(json);
        this.push(file);
        return cb();
      });
    } else {
      this.push(file);
      return cb();
    }
  };

  return through2.obj(throughCallback);
};
