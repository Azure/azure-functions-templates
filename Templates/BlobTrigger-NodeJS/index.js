module.exports = function (context, myBlob) {
    context.log("Node.js blob trigger function processed blob \n Name:", context.bindingData.name, "\n Blob Size:", myBlob.length, "Bytes");
    context.done();
};