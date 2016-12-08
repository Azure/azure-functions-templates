#r "ImageResizer.dll"
#r "System.Drawing.dll"

open System.IO
open ImageResizer

let Run(image: Stream, imageSmall: Stream, imageMedium: Stream) =
    let imageBuilder = ImageResizer.ImageBuilder.Current

    imageBuilder.Build(
        image, imageSmall, 
        ResizeSettings(320, 200, FitMode.Max, null), false)

    image.Position <- int64 0

    imageBuilder.Build(
        image, imageMedium,
        ResizeSettings(800, 600, FitMode.Max, null), false)
