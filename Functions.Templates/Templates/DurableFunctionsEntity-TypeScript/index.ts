/*
* This function is not intended to be invoked directly. Instead it will be
* triggered by a client function.
* 
* Before running this sample, please:
* - create a Durable entity HTTP function
* - run 'npm install durable-functions' from the root of your app
*/

import * as df from "durable-functions"

const entity = df.entity(function (context) {
    const currentValue = context.df.getState(() => 0) as number;
    switch (context.df.operationName) {
        case "add":
            const amount = context.df.getInput() as number;
            context.df.setState(currentValue + amount);
            break;
        case "reset":
            context.df.setState(0);
            break;
    }
});

export default entity;