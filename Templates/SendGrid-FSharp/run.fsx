#r "SendGrid"

open System
open SendGrid.Helpers.Mail

// The 'From' and 'To' fields are automatically populated with the values specified by the binding settings.
//
// You can also optionally configure the default From/To addresses globally via host.config, e.g.:
//
// {
//   "sendGrid": {
//      "to": "user@host.com",
//      "from": "Azure Functions <samples@functions.com>"
//   }
// }

[<CLIMutable>]
type Order = {
    OrderId: string
    CustomerName: string
    CustomerEmail: string
}

let Run(order: Order, log: TraceWriter) =
    log.Info(
        sprintf "F# Queue trigger function processed order: %s" order.OrderId)

    let message = new Mail()
    message.Subject <- sprintf "Thanks for your order (#%s)!" order.OrderId

    let content =
        Content(
            "text/plain",
            sprintf "%s, your order (%s) is being processed!"
                order.CustomerName order.OrderId
        )

    message.AddContent(content)
    message
