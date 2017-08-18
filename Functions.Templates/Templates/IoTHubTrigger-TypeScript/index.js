"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
function run(context, IoTHubMessages) {
    context.log("TypeScript eventhub trigger function called for message array " + IoTHubMessages);
    IoTHubMessages.forEach(function (message) {
        context.log("Processed message " + message);
    });
    context.done();
}
exports.run = run;
;
//# sourceMappingURL=index.js.map