# Register this blueprint by adding the following line of code 
# to your entry point file.  
# app.register_functions($(BLUEPRINT_FILENAME)) 
# 
# Please refer to https://aka.ms/azure-functions-python-blueprints


import azure.functions as func
import azure.durable_functions as df

$(BLUEPRINT_FILENAME) = func.Blueprint()
