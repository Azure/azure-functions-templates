import logging

import azure.functions as func


def main(blob: func.InputStream):
    # Log the name, size, URI, and contents of the blob.

    logging.info("Python Blob trigger function processed blob")

    blob_name = blob.name
    blob_size = blob.length
    blob_uri = blob.uri

    logging.info("Blob name: " + blob_name)
    logging.info("Blob size: " + blob_size + " bytes")
    logging.info("Blob URI: " + blob_uri)

    if blob.readable():
        blob_contents = blob.read().decode("utf-8")

    logging.info(blob_contents)
