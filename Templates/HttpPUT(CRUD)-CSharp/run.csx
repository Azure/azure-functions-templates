#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;

public static HttpResponseMessage Run(Person person, CloudTable outTable, TraceWriter log)
{
    if (string.IsNullOrEmpty(person.Name))
    {
        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("A non-empty Name must be specified.")
        };
    };

    log.Info($"PersonName={person.Name}");
    
    TableOperation updateOperation = TableOperation.InsertOrReplace(person);
    TableResult result = outTable.Execute(updateOperation);    
    return new HttpResponseMessage((HttpStatusCode)result.HttpStatusCode);
}

public class Person : TableEntity
{
    public string Name { get; set; }
}