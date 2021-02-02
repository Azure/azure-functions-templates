var string_decode = require('string_decoder').StringDecoder;

module.exports = async function (context, event) {
    const dec = new string_decode('utf-8');
    let event_str = dec.write(event);

    context.log.info(`JavaScript Kafka trigger function called for message ${event_str}`);
};