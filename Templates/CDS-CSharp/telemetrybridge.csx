using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.CommonDataService;
using Microsoft.CommonDataService.Common.Internal;

class TraceWriterTelemetryBridge : TraceWriter, ITelemetryService
{
    enum MessageLevel
    {
        Error,
        Warning,
        Info,
        Verbose
    }

    private readonly TraceWriter logger;

    public TraceWriterTelemetryBridge() : base(TraceLevel.Verbose)
    {
    }

    public TraceWriterTelemetryBridge(TraceWriter log) : this()
    {
        logger = log;
    }

    private void WriteToTraceWriter(string message, MessageLevel messageLevel)
    {
        switch (messageLevel)
        {
            case MessageLevel.Error:
                logger.Error(message);
                break;
            case MessageLevel.Warning:
                logger.Warning(message);
                break;
            case MessageLevel.Info:
                logger.Info(message);
                break;
            case MessageLevel.Verbose:
                logger.Verbose(message);
                break;
        }
    }

    public override void Trace(TraceEvent traceEvent)
    {
        System.Diagnostics.Trace.TraceInformation(traceEvent.Message, traceEvent.Source);
    }

    public void TraceError(string message)
    {
        WriteToTraceWriter(message, MessageLevel.Error);
    }

    public void TraceError<T0>(string format, T0 arg0)
    {
        WriteToTraceWriter(string.Format(format, arg0), MessageLevel.Error);
    }

    public void TraceError<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1), MessageLevel.Error);
    }

    public void TraceError<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1, arg2), MessageLevel.Error);
    }

    public void TraceError<T0, T1, T2, T3>(string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1, arg2, arg3), MessageLevel.Error);
    }

    public void TraceFatal(string message)
    {
        WriteToTraceWriter(message, MessageLevel.Error);
    }

    public void TraceFatal<T0>(string format, T0 arg0)
    {
        WriteToTraceWriter(string.Format(format, arg0), MessageLevel.Error);
    }

    public void TraceFatal<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1), MessageLevel.Error);
    }

    public void TraceFatal<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1, arg2), MessageLevel.Error);
    }

    public void TraceFatal<T0, T1, T2, T3>(string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1, arg2, arg3), MessageLevel.Error);
    }

    public void TraceInformation(string message)
    {
        WriteToTraceWriter(message, MessageLevel.Info);
    }

    public void TraceInformation<T0>(string format, T0 arg0)
    {
        WriteToTraceWriter(string.Format(format, arg0), MessageLevel.Info);
    }

    public void TraceInformation<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1), MessageLevel.Info);
    }

    public void TraceInformation<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1, arg2), MessageLevel.Info);
    }

    public void TraceInformation<T0, T1, T2, T3>(string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1, arg2, arg3), MessageLevel.Info);
    }

    public void TraceVerbose(string message)
    {
        WriteToTraceWriter(message, MessageLevel.Verbose);
    }

    public void TraceVerbose<T0>(string format, T0 arg0)
    {
        WriteToTraceWriter(string.Format(format, arg0), MessageLevel.Verbose);
    }

    public void TraceVerbose<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1), MessageLevel.Verbose);
    }

    public void TraceVerbose<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1, arg2), MessageLevel.Verbose);
    }

    public void TraceVerbose<T0, T1, T2, T3>(string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1, arg2, arg3), MessageLevel.Verbose);
    }

    public void TraceWarning(string message)
    {
        WriteToTraceWriter(message, MessageLevel.Warning);
    }

    public void TraceWarning<T0>(string format, T0 arg0)
    {
        WriteToTraceWriter(string.Format(format, arg0), MessageLevel.Warning);
    }

    public void TraceWarning<T0, T1>(string format, T0 arg0, T1 arg1)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1), MessageLevel.Warning);
    }

    public void TraceWarning<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1, arg2), MessageLevel.Warning);
    }

    public void TraceWarning<T0, T1, T2, T3>(string format, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        WriteToTraceWriter(string.Format(format, arg0, arg1, arg2, arg3), MessageLevel.Warning);
    }
}
