const df = require("durable-functions");

module.exports = async function (context, req) {
    const client = df.getClient(context);
    const id = context.bindingData.id;
    const entityId = new df.EntityId("Counter", id);

    if (req.method === "POST") {
        // increment value
        await client.signalEntity(entityId, "add", 1);
    } else {
        // reads current state of entity
        const stateResponse = await client.readEntityState(entityId);
        return { body: stateResponse.entityState };
    }
};