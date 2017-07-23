using System;
using Microsoft.Azure.WebJobs;

namespace webjobs_core_example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Host starting up");
            
            JobHostConfiguration jobHostHonfig = new JobHostConfiguration();
            JobHost host = new JobHost(jobHostHonfig);
            host.RunAndBlock();
        }
    }
}
