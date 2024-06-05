import azure.functions as func
import azure.durable_functions as df

dfApp = df.DFApp(http_auth_level=func.AuthLevel.ANONYMOUS)

# Activity
@dfApp.activity_trigger(input_name="city")
def $(FUNCTION_NAME_INPUT)(city: str):
    return "Hello " + city 