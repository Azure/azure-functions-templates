// Setup
// 1) Go to https://www.microsoft.com/cognitive-services/en-us/computer-vision-api 
//    Sign up for computer vision api
// 2) Go to Function app settings -> App Service settings -> Settings -> Application settings
//    create a new app setting Vision_API_Subscription_Key and use Computer vision key as value
#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"
#r "System.Net.Http"

open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.IO
open Newtonsoft.Json
open Microsoft.WindowsAzure.Storage.Table

type FaceRectangle() =
    inherit TableEntity()
    member val ImageFile: string = null with get, set
    member val Left: int = 0 with get, set
    member val Top: int = 0 with get, set
    member val Width: int = 0 with get, set
    member val Height: int = 0 with get, set

type Face = {
    Age: int
    Gender: string
    FaceRectangle: FaceRectangle
}

type ImageData = {
    Faces: Face list
}

let callVisionAPI (image: Stream) = async {
    use client = new HttpClient()
    use content = new StreamContent(image)
    let url = "https://api.projectoxford.ai/vision/v1.0/analyze?visualFeatures=Faces"

    client.DefaultRequestHeaders.Add(
        "Ocp-Apim-Subscription-Key",
        Environment.GetEnvironmentVariable("Vision_API_Subscription_Key"))
    content.Headers.ContentType <-
        new MediaTypeHeaderValue("application/octet-stream");

    let! httpResponse = client.PostAsync(url, content) |> Async.AwaitTask

    match httpResponse.StatusCode with
    | HttpStatusCode.OK -> return! httpResponse.Content.ReadAsStringAsync() |> Async.AwaitTask
    | _ -> return null
}

let handleFace face =
    let faceRectangle = face.FaceRectangle
    faceRectangle.RowKey <- Guid.NewGuid().ToString()
    faceRectangle.PartitionKey <- "Functions"
    faceRectangle.ImageFile <- name + ".jpg"
    outTable.AddAsync(faceRectangle)
    |> Async.AwaitTask
    |> Async.RunSynchronously

let Run(image: Stream, name: string, outTable: IAsyncCollector<FaceRectangle>, log: TraceWriter) =
    let result = callVisionAPI(image) |> Async.RunSynchronously
    log.Info(result)

    match String.IsNullOrEmpty(result) with
    | true -> ()
    | false ->
        let imageData = JsonConvert.DeserializeObject<ImageData>(result)
        imageData.Faces
        |> Seq.iter(fun f -> handleFace f)