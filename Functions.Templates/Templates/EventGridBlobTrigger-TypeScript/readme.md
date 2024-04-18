# EventGridBlobTrigger - TypeScript

The `EventGridBlobTrigger` makes it incredibly easy to react to new Blobs using [Event Grid](https://learn.microsoft.com/en-us/azure/event-grid/overview).
This sample demonstrates a simple use case of processing data from a given Blob using PowerShell.

## How it works

For a `EventGridBlobTrigger` to work, you provide a path which dictates where the blobs are located inside your container, and can also help restrict the types of blobs you wish to return. For instance, you can set the path to `samples/{name}.png` to restrict the trigger to only the samples path and only blobs with ".png" at the end of their name.

## Learn more

For more information, please check out [this tutorial](https://learn.microsoft.com/en-us/azure/azure-functions/functions-event-grid-blob-trigger?tabs=isolated-process%2Cnodejs-v4&pivots=programming-language-typescript) on this new version of the Blob Storage trigger.
