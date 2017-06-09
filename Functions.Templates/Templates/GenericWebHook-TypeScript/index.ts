export function run(context: any, data: IData): void {
    context.log("Webhook was triggered!");

    // check if we got first/last properties
    if ("first" in data && "last" in data) {
        context.res = {
            body: { greeting: `Hello ${data.first} ${data.last}!` }
        };
    } else {
        context.res = {
            status: 400,
            body: { error: "Please pass first/last properties in the input object" }
        };
    }

    context.done();
}

interface IData {
    first: string;
    last: string;
}

