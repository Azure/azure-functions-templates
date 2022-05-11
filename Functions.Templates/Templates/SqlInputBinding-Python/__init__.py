import azure.functions as func
import json

# Visit https://aka.ms/sqlbindingsinput to learn how to use this input binding
def main(req: func.HttpRequest, rowList: func.SqlRowList) -> func.HttpResponse:
    rows = list(map(lambda r: json.loads(r.to_json()), rowList))

    return func.HttpResponse(
        json.dumps(rows),
        status_code=200,
        mimetype="application/json"
    )