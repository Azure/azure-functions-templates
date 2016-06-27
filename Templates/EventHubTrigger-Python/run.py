import os

# read the queue message and write to stdout
input = open(os.environ['myEventHubMessage']).read()
message = "Python script processed Event Hub message '{0}'".format(input)
print(message)