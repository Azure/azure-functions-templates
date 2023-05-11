from azure.functions import FunctionApp, InputStream
import logging

app = FunctionApp()

@app.blob_trigger(arg_name="myblob", path="$(PATH_TO_BLOB_INPUT)",
                               connection="$(CONNECTION_STRING_INPUT)") 
def $(FUNCTION_NAME_INPUT)(myblob: InputStream):
    logging.info(f"Python blob trigger function processed blob \n"
                f"Name: {myblob.name}\n"
                f"Blob Size: {myblob.length} bytes")
