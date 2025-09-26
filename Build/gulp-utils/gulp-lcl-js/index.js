const { DOMParser } = require('xmldom');
var through2 = require('through2');

module.exports = function(opt) {
  opt = opt || {};

  // Convert LCL XML to JSON
  var doConvert = async function(file) {
    var xml = file.contents.toString('utf8');
    return new Promise((resolve, reject) => {
      try {
        const parser = new DOMParser();
        const doc = parser.parseFromString(xml, 'text/xml');
        const items = doc.getElementsByTagName('Item');
        const result = {};

        for (let i = 0; i < items.length; i++) {
          const item = items[i];
          const itemId = item.getAttribute('ItemId');
          
          // Skip if no ItemId or doesn't start with semicolon
          if (!itemId || !itemId.startsWith(';')) {
            continue;
          }

          // Extract the key name (remove leading semicolon)
          const key = itemId.substring(1);
          
          // Skip structural items that are not actual translations
          if (key === 'Resources.resx' || key === 'Strings') {
            continue;
          }
          
          // Find the Tgt element within this item
          const tgtElements = item.getElementsByTagName('Tgt');
          if (tgtElements.length > 0) {
            const tgtElement = tgtElements[0];
            const valElements = tgtElement.getElementsByTagName('Val');
            if (valElements.length > 0) {
              const valElement = valElements[0];
              // Extract text content, handling CDATA sections
              let value = '';
              for (let j = 0; j < valElement.childNodes.length; j++) {
                const node = valElement.childNodes[j];
                if (node.nodeType === 4) { // CDATA_SECTION_NODE
                  value += node.data;
                } else if (node.nodeType === 3) { // TEXT_NODE
                  value += node.nodeValue;
                }
              }
              result[key] = value;
            }
          }
        }

        resolve(JSON.stringify(result));
      } catch (err) {
        reject(err);
      }
    });
  };

  var throughCallback = function(file, enc, cb) {
    if (file.isStream()) {
      return cb();
    }

    if (file.isBuffer()) {
      return doConvert(file).then(json => {
        file.contents = Buffer.from(json);
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