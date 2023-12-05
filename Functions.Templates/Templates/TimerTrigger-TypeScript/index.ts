import { AzureFunction, Context, Timer } from "@azure/functions"

const timerTrigger: AzureFunction = async function (context: Context, myTimer: Timer): Promise<void> {
    var timeStamp = new Date().toISOString();
    
    if (myTimer.isPastDue)
    {
        context.log('Timer function is running late!');
    }
    context.log('Timer trigger function ran!', timeStamp);   
};

export default timerTrigger;
