# Register this blueprint by adding the following line of code 
# to your entry point file.  
# app.register_functions($(BLUEPRINT_FILENAME)) 
# 
# Please refer to https://aka.ms/azure-functions-python-blueprints
import logging
import azure.functions as func

$(BLUEPRINT_FILENAME) = func.Blueprint()
