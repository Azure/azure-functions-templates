import azure.functions as func
import collections

# Visit https://aka.ms/sqlbindingsoutput to learn how to use this output binding
def main(req: func.HttpRequest, output: func.Out[func.SqlRow]) -> func.HttpResponse:
    row = func.SqlRow(TodoItem(req.params["id"], req.params["priority"],req.params["description"]))
    output.set(row)
    return func.HttpResponse(
        row.to_json(),
        status_code=201,
        mimetype="application/json"
    )

class TodoItem(collections.UserDict):
    def __init__(self, id, priority, description):
        super().__init__()
        self['Id'] = id
        self['Priority'] = priority
        self['Description'] = description