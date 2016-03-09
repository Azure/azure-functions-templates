module.exports = function (context) {
    var timeStamp = new Date().toISOString();
    context.log('Node.js timer trigger function ran! ' + timeStamp);   
    context.done();
}