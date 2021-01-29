import { AzureFunction, Context } from "@azure/functions"

// This is to describe the metadata of a Kafka event
class KafkaEvent {
    Offset : number;
    Partition : number;
    Topic : string;
    Timestamp : string;
    Value : string;
    
    constructor(metadata:any) {
        this.Offset = metadata.Offset;
        this.Partition = metadata.Partition;
        this.Topic = metadata.Topic;
        this.Timestamp = metadata.Timestamp;
        this.Value = metadata.Value;
    }

    public getValue<T>() : T {
        return JSON.parse(this.Value).payload;
    }
}

const kafkaTrigger: AzureFunction = async function (context: Context, event_str: string): Promise<void> {

    let event_obj = new KafkaEvent(eval(event_str));

    context.log("Event Offset: " + event_obj.Offset);
    context.log("Event Partition: " + event_obj.Partition);
    context.log("Event Topic: " + event_obj.Topic);
    context.log("Event Timestamp: " + event_obj.Timestamp);
    context.log("Event Value (as string): " + event_obj.Value);
};

export default kafkaTrigger;