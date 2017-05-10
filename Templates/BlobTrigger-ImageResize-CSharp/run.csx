#r "Microsoft.WindowsAzure.Storage"
#r "System.Drawing"

// manually reference binary from NuGet package ImageResizer (https://www.nuget.org/packages/ImageResizer/),
// since NuGet restore does not currently work for Templates.
#r "ImageResizer.dll" 

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using ImageResizer;

public enum ImageSize
{
    ExtraSmall, Small, Medium, Large
}

#region Image dimensions

private static Dictionary<ImageSize, ResizeSettings> imageDimensionsTable = new Dictionary<ImageSize, ResizeSettings>()
{
    { ImageSize.ExtraSmall, new ResizeSettings("maxwidth=320&maxheight=200") },
    { ImageSize.Small, new ResizeSettings("maxwidth=640&maxheight=400") },
    { ImageSize.Medium, new ResizeSettings("maxwidth=800&maxheight=600") }
};

#endregion

public static void Run(
    Stream image,                           // input blob, large size
    Stream imageSmall, Stream imageMedium)  // output blobs
{
    var imageBuilder = ImageResizer.ImageBuilder.Current;
    imageBuilder.Build(image, imageSmall, imageDimensionsTable[ImageSize.Small], disposeSource: false);
    
    image.Position = 0;
    
    imageBuilder.Build(image, imageMedium, imageDimensionsTable[ImageSize.Medium], disposeSource: false);
}


