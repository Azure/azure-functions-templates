open System.IO
open ImageResizer

let Run(imageIn: Stream, imageSmall: Stream, imageMedium: Stream) =
    let imageBuilder = ImageResizer.ImageBuilder.Current

    imageBuilder.Build(
        imageIn, imageSmall, 
        ResizeSettings(320, 200, FitMode.Max, null), false)

    imageBuilder.Build(
        imageIn, imageMedium,
        ResizeSettings(800, 600, FitMode.Max, null), false)
