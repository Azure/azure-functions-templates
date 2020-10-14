import logging
import json

import azure.functions as func
import azure.durable_functions as df


def entity_function(context: df.DurableOrchestrationContext):

    current_value = context.get_state(lambda: 0)
    operation = context.operation_name
    if operation == "add":
        amount = context.get_input()
        current_value += amount
        context.set_result(current_value)
    elif operation == "reset":
        current_value = 0
    elif operation == "get":
        context.set_result(current_value)
    
    context.set_state(current_value)

main = df.Entity.create(entity_function)