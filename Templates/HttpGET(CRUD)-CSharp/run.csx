#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;

public static HttpResponseMessage Run(HttpRequestMessage req, IQueryable<Person> inTable, TraceWriter log)
{
    var query = from person in inTable select person;
    foreach (Person person in query)
    {
        log.Info($"Name:{person.Name}");
    }
    return req.CreateResponse(HttpStatusCode.OK, inTable.ToList());
}

public class Person : TableEntity
{
    public string Name { get; set; }
}