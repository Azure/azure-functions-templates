module.exports = function (context, timerinfo) {
    var timeStamp = new Date().toISOString();
    
    if(timerinfo.isPastDue)
    {
        context.log('Node.js is running late!');
    }
    context.log('Node.js timer trigger function ran! ' + timeStamp);   
    
    context.done();
}