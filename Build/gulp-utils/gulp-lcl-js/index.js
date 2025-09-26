const { DOMParser } = require('@xmldom/xmldom');
var through2 = require('through2');

// Structural items that should be excluded from translation processing
const STRUCTURAL_ITEMS = [
  'Resources.resx',
  'Strings'
];

// Default prefix for translation entry ItemIds in LCL format
const DEFAULT_TRANSLATION_PREFIX = ';';

module.exports = function(opt) {
  opt = opt || {};
  
  // Allow override of structural items through options
  const structuralItems = opt.excludeItems || STRUCTURAL_ITEMS;
  
  // Allow override of the translation entry prefix
  const translationPrefix = opt.translationPrefix || DEFAULT_TRANSLATION_PREFIX;

  // Convert LCL XML to JSON
  var doConvert = function(file) {
    var xml = file.contents.toString('utf8');
    return new Promise((resolve, reject) => {
      try {
        const parser = new DOMParser();
        const doc = parser.parseFromString(xml, 'text/xml');
        const items = doc.getElementsByTagName('Item');
        const result = {};
        const duplicateKeys = new Set();

        for (let i = 0; i < items.length; i++) {
          const item = items[i];
          const itemId = item.getAttribute('ItemId');
          
          // Only process items whose ItemId starts with the translation prefix.
          // In LCL XML, ItemId values starting with the prefix (default ';') represent actual translation entries,
          // while others are structural or metadata items and should be skipped.
          if (!itemId || !itemId.startsWith(translationPrefix)) {
            continue;
          }

          // Extract the key name (remove leading prefix)
          const key = itemId.substring(translationPrefix.length);
          
          // Skip structural items that are not actual translations
          if (structuralItems.includes(key)) {
            continue;
          }
          
          // Check for duplicate keys
          if (result.hasOwnProperty(key)) {
            duplicateKeys.add(key);
            console.warn(`[LCL Parser] Duplicate key detected: "${key}" - previous value will be overwritten`);
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
            } else {
              console.warn(`[LCL Parser] Item "${key}" has <Tgt> element but missing <Val> child - skipping item`);
            }
          } else {
            console.warn(`[LCL Parser] Item "${key}" missing <Tgt> element - skipping item`);
          }
        }

        // Log summary of duplicate keys if any were found
        if (duplicateKeys.size > 0) {
          console.warn(`[LCL Parser] Found ${duplicateKeys.size} duplicate keys: ${Array.from(duplicateKeys).join(', ')}`);
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