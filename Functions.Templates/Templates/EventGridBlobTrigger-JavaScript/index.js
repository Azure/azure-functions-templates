module.exports = async function (context, myEventGridBlob) {
    context.log("JavaScript blob storage trigger (using Event Grid) function processed blob \n Blob:", context.bindingData.blobTrigger, "\n Blob Size:", myEventGridBlob.length, "Bytes");
};