#r "Microsoft.WindowsAzure.Storage"
#r "System.Drawing"

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

// Use the ImageResizer NuGet to resize images based a blob trigger.
// Currently, NuGet restore doesn't work for templates. To trigger a restore manually, go to your Kudu site and touch project.json. 
// Go to http://yoursite.scm.azurewebsites.net/DebugConsole, then edit site/wwwroot/ImageResizerCSharp/project.json.
// Or, use the version based on System.Drawing by commenting out this method and using statement and uncommenting the one below.
using ImageResizer;

public static void Run(
    Stream image,                           // input blob, large size
    Stream imageSmall, Stream imageMedium)  // output blobs
{
    var imageBuilder = ImageResizer.ImageBuilder.Current;
    var size = imageDimensionsTable[ImageSize.Small];

    imageBuilder.Build(
        image, imageSmall, 
        new ResizeSettings(size.Width, size.Height, FitMode.Max, null), false);

    image.Position = 0;
    size = imageDimensionsTable[ImageSize.Medium];

    imageBuilder.Build(
        image, imageMedium,
        new ResizeSettings(size.Width, size.Height, FitMode.Max, null), false);
}

// Image resize based on System.Drawing. Do NOT use in production!  
// See http://www.asprangers.com/post/2012/03/23/Why-you-should-not-use-SystemDrawing-from-ASPNET-applications.aspx 
// public static void Run(
//     Stream image,                           // input blob, large size
//     Stream imageSmall, Stream imageMedium)  // output blobs
// {
//     ScaleImage(image, imageSmall, ImageSize.Small);
//     ScaleImage(image, imageMedium, ImageSize.Medium);
// }

#region Helpers

public enum ImageSize
{
    ExtraSmall, Small, Medium
}

private static Dictionary<ImageSize, Size> imageDimensionsTable = new Dictionary<ImageSize, Size>()
{
    { ImageSize.ExtraSmall, new Size(320, 200) },
    { ImageSize.Small, new Size(640, 400) },
    { ImageSize.Medium, new Size(800, 600) }
};

private static ImageFormat ScaleImage(Stream blobInput, Stream output, ImageSize imageSize)
{
    ImageFormat imageFormat;

    var size = imageDimensionsTable[imageSize];

    blobInput.Position = 0;

    using (var img = System.Drawing.Image.FromStream(blobInput)) {
        var widthRatio = (double)size.Width / (double)img.Width;
        var heightRatio = (double)size.Height / (double)img.Height;
        var minAspectRatio = Math.Min(widthRatio, heightRatio);
        if (minAspectRatio > 1) {
            size.Width = img.Width;
            size.Width = img.Height;
        }
        else {
            size.Width = (int)(img.Width * minAspectRatio);
            size.Height = (int)(img.Height * minAspectRatio);
        }

        using (Bitmap bitmap = new Bitmap(img, size)) {
            bitmap.Save(output, img.RawFormat);
            imageFormat = img.RawFormat;
        }
    }

    return imageFormat;
}

#endregion