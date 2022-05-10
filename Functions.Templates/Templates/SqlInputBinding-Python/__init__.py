# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

import azure.functions as func
import json

def main(req: func.HttpRequest, rowList: func.SqlRowList) -> func.HttpResponse:
    rows = list(map(lambda r: json.loads(r.to_json()), rowList))

    return func.HttpResponse(
        json.dumps(rows),
        status_code=200,
        mimetype="application/json"
    )