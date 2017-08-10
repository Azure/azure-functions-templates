#if (portalTemplates)
using ImageResizer;

public static void Run(
    Stream image,                           // input blob, large size
    Stream imageSmall, Stream imageMedium)  // output blobs
#endif
#if (vsTemplates)
using System;
using System.Collections.Generic;
using System.IO;
using ImageResizer;
using Microsoft.Azure.WebJobs;

namespace Company.Function
{
    public static class ImageResizerCSharp
    {
        [FunctionName("ImageResizerCSharp")]
        public static void Run([BlobTrigger("ImageBlobPathValue", Connection = "ImageBlobConnectionValue")]Stream image, [Blob("SmallImageBlobPathValue", Connection = "SmallImageBlobConnectionValue")]Stream imageSmall, [Blob("MedImageBlobPathValue", Connection = "MedImageBlobConnectionValue")]Stream imageMedium)  // output blobs
#endif
        {
            var imageBuilder = ImageResizer.ImageBuilder.Current;
            var size = imageDimensionsTable[ImageSize.Small];

            imageBuilder.Build(
                image, imageSmall,
                new ResizeSettings(size.Item1, size.Item2, FitMode.Max, null), false);

            image.Position = 0;
            size = imageDimensionsTable[ImageSize.Medium];

            imageBuilder.Build(
                image, imageMedium,
                new ResizeSettings(size.Item1, size.Item2, FitMode.Max, null), false);
        }

        public enum ImageSize
        {
            ExtraSmall, Small, Medium
        }

        private static Dictionary<ImageSize, Tuple<int, int>> imageDimensionsTable = new Dictionary<ImageSize, Tuple<int, int>>()
        {
            { ImageSize.ExtraSmall, Tuple.Create(320, 200) },
            { ImageSize.Small,      Tuple.Create(640, 400) },
            { ImageSize.Medium,     Tuple.Create(800, 600) }
        };
#if (vsTemplates)
    }
}
#endif