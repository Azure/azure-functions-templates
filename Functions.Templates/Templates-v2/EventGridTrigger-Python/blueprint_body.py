@$(BLUEPRINT_FILENAME).event_grid_trigger(arg_name="azeventgrid")
def $(FUNCTION_NAME_INPUT)(azeventgrid: func.EventGridEvent):
    logging.info('Python EventGrid trigger processed an event')
