import os
import json

with open(os.environ['req']) as requestFile:
    postreqdata = json.loads(requestFile.read())

with open(os.environ['res'], 'w') as response:
    response.write("hello world from "+postreqdata['name'])
