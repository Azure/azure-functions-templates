import os

# read the queue message and write to stdout
with open(os.environ['inputMessage']) as inputFile:
    inputMessage = inputFile.read()
message = "Python script processed queue message '{0}'".format(inputMessage)
print(message)
