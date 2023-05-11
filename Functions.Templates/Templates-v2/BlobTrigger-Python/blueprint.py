# Register this blueprint by adding the following line of code 
# to your entry point file.  
# app.register_functions($(BLUEPRINT_FILENAME)) 
# 
#Please refer to https://aka.ms/azure-functions-python-blueprints


from azure.functions import Blueprint
import logging

$(BLUEPRINT_FILENAME) = Blueprint()