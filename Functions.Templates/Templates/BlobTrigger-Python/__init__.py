import logging

import azure.functions as func


def main(blob: func.InputStream):
    logging.info(
        f"Python blob trigger function processed blob \n"
        f"Name: {blob.name}\n"
        f"Blob Size: {blob.length} bytes"
    )
