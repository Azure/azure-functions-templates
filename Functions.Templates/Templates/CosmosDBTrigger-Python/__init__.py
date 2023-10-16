import logging

from azure.functions import DocumentList

# This template uses an outdated version of the Azure Cosmos DB extension. Learn about migrating to the new extension at https://aka.ms/migrate-to-cosmos-extension-v4
def main(documents: DocumentList) -> str:
    if documents:
        logging.info('Document id: %s', documents[0]['id'])
