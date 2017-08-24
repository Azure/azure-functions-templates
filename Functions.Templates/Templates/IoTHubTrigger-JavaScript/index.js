module.exports = function (context, IoTHubMessages) {
    context.log(`JavaScript eventhub trigger function called for message array ${IoTHubMessages}`);
    
    IoTHubMessages.forEach(message => {
        context.log(`Processed message ${message}`);
    });

    context.done();
};