import azure.functions as func
import logging

app = func.FunctionApp()

@app.blob_trigger(arg_name="myblob", path="$(PATH_TO_BLOB_INPUT)",
                               connection="$(CONNECTION_STRING_INPUT)") 
def $(FUNCTION_NAME_INPUT)(myblob: func.InputStream):
    logging.info(f"Python blob trigger function processed blob"
                f"Name: {myblob.name}"
                f"Blob Size: {myblob.length} bytes")
