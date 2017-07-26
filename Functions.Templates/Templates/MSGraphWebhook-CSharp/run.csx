#r "O365Extensions"
#r "Microsoft.Graph"

using System;
using Microsoft.Graph;
using O365Extensions;

public static void Run(Message msg, TraceWriter log)
{ 
    log.Info($"Received email with subject: {msg.Subject} at {msg.SentDateTime.ToString()}");
}