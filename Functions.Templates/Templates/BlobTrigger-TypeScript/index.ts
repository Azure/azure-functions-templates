export function run(context: any, myBlob: any): void {
    context.log(`TypeScript blob trigger function processed blob 
Name: ${context.bindingData.name}
Blob Size: ${myBlob.length} Bytes`);
    context.done();
};