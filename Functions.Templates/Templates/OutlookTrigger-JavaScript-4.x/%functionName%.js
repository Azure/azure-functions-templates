const { DeliveryMode } = require('@azure/functions');
const { connectors } = require('@azure/functions-extensions-connectors');

// Connector operations and normalized payload types (Office365Extensions):
// OnNewEmailV3 - GraphClientReceiveMessage[]
// OnFlaggedEmailV4 - GraphClientReceiveMessage[]
// OnNewMentionMeEmailV3 - GraphClientReceiveMessage[]
// SharedMailboxOnNewEmailV2 - GraphClientReceiveMessage[]
// CalendarGetOnNewItemsV3 - GraphCalendarEventClientReceive[]
// CalendarGetOnUpdatedItemsV3 - GraphCalendarEventClientReceive[]
// OnUpcomingEventsV3 - GraphCalendarEventClientReceive[]
// CalendarGetOnChangedItemsV3 - GraphCalendarEventClientWithActionType[]
// Use the matching typed helper or connectorTrigger<TItem> for another operation.
// Configure a Poll trigger config with that operation's required parameters.
connectors.office365.onNewEmail('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnNewEmailTriggerConfigName%',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.emails.length} Office 365 email(s).`);
    }
});
