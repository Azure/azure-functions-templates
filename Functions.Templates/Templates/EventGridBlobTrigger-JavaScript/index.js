module.exports = async function (context, myEventGridBlob) {
    context.log("JavaScript Event Grid blob trigger function processed blob \n Blob:", context.bindingData.blobTrigger, "\n Blob Size:", myEventGridBlob.length, "Bytes");
};