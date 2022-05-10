# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

import azure.functions as func
import collections

def main(req: func.HttpRequest, product: func.Out[func.SqlRow]) -> func.HttpResponse:
    row = func.SqlRow(Product(req.params["id"], req.params["name"],req.params["cost"]))
    product.set(row)
    return func.HttpResponse(
        row.to_json(),
        status_code=201,
        mimetype="application/json"
    )

class Product(collections.UserDict):
    def __init__(self, productId, name, cost):
        super().__init__()
        self['ProductId'] = productId
        self['Name'] = name
        self['Cost'] = cost