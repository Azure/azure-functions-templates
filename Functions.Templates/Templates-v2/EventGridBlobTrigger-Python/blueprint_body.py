
@$(BLUEPRINT_FILENAME).blob_trigger(arg_name="myblob", path="$(PATH_TO_BLOB_INPUT)", source=func.BlobSource.EVENT_GRID,
                               connection="$(CONNECTION_STRING_INPUT)") 
def $(FUNCTION_NAME_INPUT)(myblob: func.InputStream):
    logging.info(f"Python blob trigger (using Event Grid) function processed blob"
                f"Name: {myblob.name}"
                f"Blob Size: {myblob.length} bytes")