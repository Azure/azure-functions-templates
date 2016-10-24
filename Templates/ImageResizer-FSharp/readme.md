# Image resizing with a blob trigger - F<span>#</span>

Using a blob trigger and the [ImageResizer](https://www.nuget.org/packages/ImageResizer/) nuget package, you can create a function that resizes new images when they are added to a container. This sample shows how to use a blob trigger to trigger the function and how to use output blobs with the same input blob name.

## How it works

For a `BlobTrigger` to work, you provide a path which dictates where the blobs are located inside your container, and can also help restrict the types of blobs you wish to return. For instance, you can set the path to `samples/{name}.png` to restrict the trigger to only the samples path and only blobs with ".png" at the end of their name.

The sample uses different containers for the different image sizes, but you could instead use the same container and rename the image file instead. For instance, the blob path `sample-images/md-{name}` would prepend "md" to the input blob name.

