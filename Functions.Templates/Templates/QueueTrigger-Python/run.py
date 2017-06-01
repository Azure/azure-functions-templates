import os

# read the queue message and write to stdout
inputMessage = open(os.environ['inputMessage']).read()
message = "Python script processed queue message '{0}'".format(inputMessage)
print(message)