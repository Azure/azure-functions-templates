import logging

from azure.functions import DocumentList


def main(documents: DocumentList) -> str:
    if documents:
        logging.info('Document id: %s', documents[0]['id'])
