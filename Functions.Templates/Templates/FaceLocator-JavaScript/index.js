// Setup
// 1) Go to https://www.microsoft.com/cognitive-services/en-us/computer-vision-api 
//    Sign up for computer vision api
// 2) Go to Platform features -> Application settings
//    create a new app setting Vision_API_Subscription_Key and use Computer vision key as value
// 3) Go to Platform features -> Console
//    Enter the following commands: 
//    > cd <functionName>
//    > npm install

var request = require('request-promise');
var guid = require('node-uuid');

module.exports = function (context, image) {

    context.log("Image Size:", image.length);
    var options = {
        uri: "https://westus.api.cognitive.microsoft.com/vision/v1.0/analyze?visualFeatures=Faces&language=en",
        method: 'POST',
        body: image,
        headers: {
            'Content-Type': 'application/octet-stream',
            'Ocp-Apim-Subscription-Key': process.env.Vision_API_Subscription_Key
        }
    };

    request(options)
        .then((response) => {
            response = JSON.parse(response);

            if (!response || !response.faces.length) {
                return context.done();
            }

            context.log("From Vision Api:", response);

            context.bindings.outTable = response.faces.map((face) => {
                var faceMapping = {
                    rowKey: guid.v1(),
                    imageFile: context.bindingData.name + ".jpg",
                    partitionKey: "Functions"
                };
                Object.assign(faceMapping, face.faceRectangle);
                return faceMapping;
            });
        })
        .catch((error) => context.log(error))
        .finally(() => context.done());
};