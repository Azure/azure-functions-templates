import logging

from azure.functions import HttpRequest, HttpResponse, Out


def main(req: HttpRequest, out: Out[str]) -> HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    message = req.params.get('message')
    if not message:
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            message = req_body.get('message')

    if message:
        out.set(message)
        return HttpResponse(f"Message received: {message}. The message transfered to the kafka broker.")
    else:
        return HttpResponse(
            "This HTTP triggered function executed successfully but no message was passed. Please pass message as request parameter or in body for sending data to Kafka",
            status_code=200
        )
