module.exports = async function (context, myBlob) {
    context.log("JavaScript Event Grid blob trigger function processed blob \n Blob:", context.bindingData.blobTrigger, "\n Blob Size:", myBlob.length, "Bytes");
};