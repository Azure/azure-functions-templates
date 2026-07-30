# Template Manifest — Priority Tiers (P00–P99)

**Total templates:** 78
**Manifest version:** 1.10.0

## Design principles

1. **Priority is the primary sort key.** Lower number = show first.
2. **5-slot blocks per Azure resource:** `+0` trigger, `+1` input, `+2` output, `+3` variant, `+4` stub.
3. **10-slot block for MCP:** `+0` Remote Server, `+1` SDK Hosting, `+2` Tool, `+3` Resource, `+4` Prompt, `+5` APIM Gateway.
4. **Language sort order (secondary):** .NET (C#) → Python → TypeScript → JavaScript → Java → PowerShell → Go.
   Templates sharing a priority are sorted by this language order by the consumer.
5. **IaC sort order (tertiary):** 🚧 no AZD → ✅ AZD → 📦 IaC-only.
   no AZD = what VS Code Azure Functions extension generates = the familiar baseline.
6. **Default rule:** New templates without AZD/IaC default to P95–P99.

## Priority block map

| Range | Category | Supported bindings | Slot assignments |
|---|---|---|---|
| P00–04 | ⚡ HTTP | Trigger · Output | `P00` Trigger<br>`P01` Input *(N/A)*<br>`P02` Output *(future)*<br>`P03` Variant (Terraform)<br>`P04` Stub |
| P05–09 | ⏰ Timer | Trigger only | `P05` Trigger |
| P10–14 | 🪣 Blob Storage | Trigger · Input · Output | `P10` Trigger<br>`P11` Input *(future)*<br>`P12` Output *(future)* |
| P15–19 | 📡 Event Hub | Trigger · Output | `P15` Trigger<br>`P16` Output *(future)* |
| P20–24 | 📣 Event Grid *(reserved)* | Trigger · Output | `P20` Trigger<br>`P21` Output |
| P25–29 | 📬 Queue Storage *(reserved)* | Trigger · Output | `P25` Trigger<br>`P26` Output |
| P30–34 | 🚌 Service Bus | Trigger · Output | `P30` Trigger<br>`P31` Output |
| P35–39 | 🌐 Cosmos DB | Trigger · Input · Output | `P35` Trigger<br>`P36` Input *(future)*<br>`P37` Output *(future)* |
| P40–44 | 🗄️ SQL | Trigger · Input · Output | `P40` Trigger<br>`P41` Input *(future)*<br>`P42` Output *(future)* |
| P45–49 | 🔴 Redis *(reserved)* | Trigger · Input · Output | `P45` Trigger<br>`P46` Input<br>`P47` Output |
| P50–59 | 🔌 MCP *(10-slot block)* | Trigger (Tool / Resource / Prompt) | `P50` Remote Server<br>`P51` SDK Hosting *(removed — repos archived)*<br>`P52` Tool<br>`P53` Resource<br>`P54` Prompt<br>`P55` APIM Gateway<br>`P56` *(reserved)*<br>`P57` *(reserved)*<br>`P58` *(reserved)*<br>`P59` *(reserved)* |
| P60–64 | 🤖 AI | — | `P60` Agent<br>`P61` ChatGPT<br>`P62` Text Summarize *(removed — repos archived)*<br>`P63` LangChain |
| P65–69 | 🔄 Durable Standard | Orchestration | `P65` Orchestration<br>`P66` Order Processor |
| P70–74 | 🔄 Durable Advanced | Orchestration | `P70` Patterns (saga / tracing / payload)<br>`P71` Scenarios (travel / aspire)<br>`P72` PDF Summarizer |
| P75–79 | 🤝 Agent Framework | Orchestration | `P75` Multi-Agent |
| P80–89 | 🔌 Connectors | Trigger | `P80` Office 365 Outlook<br>`P81` SharePoint Online |
| P90–94 | 🏗️ IaC | — | `P90` Flex Consumption (ARM / Bicep / TF) |
| P95–99 | ⚠️ Non-AZD Stubs | — | `P95` Gap-fillers (AZD upgrade planned) |

## Full template listing

Sorted by priority → language order (.NET → Py → TS → JS → Java → PS → Go).

| P | Template ID | Lang | Binding | IaC | Was |
|--:|---|---|---|---|---|
| | **⚡ HTTP** | | | | |
| 0 | `http-trigger-csharp-azd` | .NET | trigger | ✅ bicep | P0 |
| 0 | `http-trigger-python-azd` | Py | trigger | ✅ bicep | P0 |
| 0 | `http-trigger-typescript-azd` | TS | trigger | ✅ bicep | P0 |
| 0 | `http-trigger-javascript-azd` | JS | trigger | ✅ bicep | P0 |
| 0 | `http-trigger-java-azd` | Java | trigger | ✅ bicep | P0 |
| 0 | `http-trigger-powershell-azd` | PS | trigger | ✅ bicep | P0 |
| 0 | `http-trigger-go-azd` | Go | trigger | ✅ bicep | *new* |
| 3 | `http-trigger-csharp-terraform` | .NET | trigger | 📦 terraform | P0→3 |
| | **⏰ Timer** | | | | |
| 5 | `timer-trigger-csharp-azd` | .NET | trigger | ✅ bicep | P0→5 |
| 5 | `timer-trigger-python-azd` | Py | trigger | ✅ bicep | P0→5 |
| 5 | `timer-trigger-typescript-azd` | TS | trigger | ✅ bicep | P0→5 |
| 5 | `timer-trigger-javascript-azd` | JS | trigger | ✅ bicep | P0→5 |
| 5 | `timer-trigger-java-azd` | Java | trigger | ✅ bicep | P0→5 |
| 5 | `timer-trigger-powershell-azd` | PS | trigger | ✅ bicep | P0→5 |
| | **📦 Blob Storage** | | | | |
| 10 | `blob-eventgrid-trigger-csharp-azd` | .NET | trigger | ✅ bicep | P0→10 |
| 10 | `blob-eventgrid-trigger-python-azd` | Py | trigger | ✅ bicep | P0→10 |
| 10 | `blob-eventgrid-trigger-typescript-azd` | TS | trigger | ✅ bicep | P0→10 |
| 10 | `blob-eventgrid-trigger-javascript-azd` | JS | trigger | ✅ bicep | P0→10 |
| 10 | `blob-eventgrid-trigger-java-azd` | Java | trigger | ✅ bicep | P0→10 |
| 10 | `blob-eventgrid-trigger-powershell-azd` | PS | trigger | ✅ bicep | P0→10 |
| | **📡 Event Hub** | | | | |
| 15 | `eventhub-trigger-csharp-azd` | .NET | trigger | ✅ bicep | P0→15 |
| 15 | `eventhub-trigger-python-azd` | Py | trigger | ✅ bicep | P0→15 |
| 15 | `eventhub-trigger-typescript-azd` | TS | trigger | ✅ bicep | P0→15 |
| 15 | `eventhub-trigger-javascript-azd` | JS | trigger | ✅ bicep | *new* |
| 15 | `eventhub-trigger-java-azd` | Java | trigger | ✅ bicep | *new* |
| 15 | `eventhub-trigger-powershell-azd` | PS | trigger | ✅ bicep | *new* |
| | **🚌 Service Bus** | | | | |
| 30 | `servicebus-trigger-csharp-azd` | .NET | trigger | ✅ bicep | *new* |
| 30 | `servicebus-trigger-python-azd` | Py | trigger | ✅ bicep | *new* |
| 30 | `servicebus-trigger-typescript-azd` | TS | trigger | ✅ bicep | *new* |
| 30 | `servicebus-trigger-javascript-azd` | JS | trigger | ✅ bicep | *new* |
| 30 | `servicebus-trigger-java-azd` | Java | trigger | ✅ bicep | *new* |
| 30 | `servicebus-trigger-powershell-azd` | PS | trigger | ✅ bicep | *new* |
| | **🌐 Cosmos DB** | | | | |
| 35 | `cosmos-trigger-csharp-azd` | .NET | trigger | ✅ bicep | P1→35 |
| 35 | `cosmos-trigger-python-azd` | Py | trigger | ✅ bicep | P1→35 |
| 35 | `cosmos-trigger-typescript-azd` | TS | trigger | ✅ bicep | P1→35 |
| 35 | `cosmosdb-trigger-javascript-azd` | JS | trigger | ✅ bicep | *new* |
| 35 | `cosmosdb-trigger-java-azd` | Java | trigger | ✅ bicep | *new* |
| 35 | `cosmosdb-trigger-powershell-azd` | PS | trigger | ✅ bicep | *new* |
| | **🗄️ SQL** | | | | |
| 40 | `sql-trigger-csharp-azd` | .NET | trigger | ✅ bicep | P1→40 |
| 40 | `sql-trigger-python-azd` | Py | trigger | ✅ bicep | P1→40 |
| 40 | `sql-trigger-typescript-azd` | TS | trigger | ✅ bicep | P1→40 |
| | **🔌 MCP *(10-slot block)*** | | | | |
| 50 | `mcp-server-remote-csharp` | .NET | trigger | ✅ bicep | P0→50 |
| 50 | `mcp-server-remote-python` | Py | trigger | ✅ bicep | P0→50 |
| 50 | `mcp-server-remote-typescript` | TS | trigger | ✅ bicep | P0→50 |
| 50 | `mcp-server-remote-javascript` | JS | trigger | ✅ bicep | *new* |
| 50 | `mcp-server-remote-java` | Java | trigger | ✅ bicep | P0→50 |
| 50 | `mcp-server-remote-go` | Go | trigger | ✅ bicep | *new* |
| 55 | `mcp-server-apim-python` | Py | trigger | ✅ bicep | P2→55 |
| | **🤖 AI** | | | | |
| 60 | `ai-agent-csharp` | .NET | trigger | ✅ bicep | P0→60 |
| 60 | `ai-agent-python` | Py | trigger | ✅ bicep | P0→60 |
| 60 | `ai-serverless-agents-python` | Py | trigger | ✅ bicep | *new* |
| 60 | `ai-agent-typescript` | TS | trigger | ✅ bicep | P0→60 |
| 60 | `ai-agent-java` | Java | trigger | ✅ bicep | P0→60 |
| 61 | `ai-chatgpt-python` | Py | trigger | ✅ bicep | P1→61 |
| 61 | `ai-chatgpt-javascript` | JS | trigger | ✅ bicep | P1→61 |
| 63 | `ai-langchain-python` | Py | trigger | ✅ bicep | P2→63 |
| | **🔄 Durable Standard** | | | | |
| 65 | `durable-orchestrator-csharp-azd` | .NET | orchestration | ✅ bicep | *new* |
| 65 | `durable-orchestrator-python-azd` | Py | orchestration | ✅ bicep | *new* |
| 65 | `durable-orchestrator-typescript-azd` | TS | orchestration | ✅ bicep | *new* |
| 65 | `durable-orchestration-javascript` | JS | orchestration | 🚧 none | P1→65 |
| 66 | `durable-order-processor-csharp` | .NET | orchestration | ✅ bicep | P1→66 |
| 66 | `durable-order-processor-python` | Py | orchestration | ✅ bicep | P1→66 |
| | **🔄 Durable Advanced** | | | | |
| 70 | `durable-distributed-tracing-csharp` | .NET | orchestration | 🚧 none | P1→70 |
| 70 | `durable-large-payload-csharp` | .NET | orchestration | 🚧 none | P1→70 |
| 70 | `durable-large-payload-fan-out-fan-in-csharp` | .NET | orchestration | ✅ bicep | *new* |
| 70 | `durable-saga-csharp` | .NET | orchestration | 🚧 none | P1→70 |
| 71 | `durable-ai-travel-planner-csharp` | .NET | orchestration | 🚧 none | P2→71 |
| 71 | `durable-aspire-csharp` | .NET | orchestration | 🚧 none | P2→71 |
| 72 | `durable-pdf-summarizer-csharp` | .NET | orchestration | 🚧 none | P2→72 |
| 72 | `durable-pdf-summarizer-python` | Py | orchestration | 🚧 none | P2→72 |
| | **🤝 Agent Framework** | | | | |
| 75 | `agentframework-durable-multi-agent-python` | Py | orchestration | 🚧 none | P1→75 |
| 75 | `agentframework-multi-agent-go` | Go | trigger | ✅ bicep | *new* |
| | **🔌 Connectors** | | | | |
| 80 | `office365-connector-trigger-csharp` | .NET | trigger | ✅ bicep | *new* |
| 80 | `office365-connector-trigger-python` | Py | trigger | ✅ bicep | *new* |
| 80 | `office365-connector-trigger-typescript` | TS | trigger | ✅ bicep | *new* |
| 81 | `sharepoint-connector-trigger-csharp` | .NET | trigger | ✅ bicep | *new* |
| 81 | `sharepoint-connector-trigger-python` | Py | trigger | ✅ bicep | *new* |
| 81 | `sharepoint-connector-trigger-typescript` | TS | trigger | ✅ bicep | *new* |

### Removed (archived repos)

These templates were removed because their source repositories were **archived**:

| Removed template(s) | P | Archived repo(s) |
|---|--:|---|
| `mcp-sdk-hosting-csharp` / `-python` / `-typescript` / `-java` | P51 | `Azure-Samples/mcp-sdk-functions-hosting-{dotnet,python,node,java}` |
| `ai-textsummarize-csharp` | P62 | `Azure-Samples/function-csharp-ai-textsummarize` |
| `ai-textsummarize-python` | P62 | `Azure-Samples/function-python-ai-textsummarize` |

## Coverage matrix

**Icon legend** (matches IaC sort order in design principle 5):

| Icon | Meaning |
|------|---------|
| ✅ | AZD (azure.yaml + Bicep) |
| 📦 | IaC-only (Bicep / Terraform / ARM, no azure.yaml) |
| 🔗 | Covered indirectly via trigger sample |
| 🔗+🚧 | Covered indirectly via trigger sample (no AZD) |
| 🚧 | No AZD |
| — | No template or reserved |

### Azure resource templates (P00–P44)

| Resource | Binding | .NET | Py | TS | JS | Java | PS | Go | Gaps |
|---|---|---|---|---|---|---|---|---|---|
| HTTP | Trigger | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | — |
| Timer | Trigger | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | — | Go |
| Blob | Trigger | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | — | Go |
| | Input | 🔗+🚧 | 🔗+🚧 | 🔗+🚧 | 🔗+🚧 | 🔗+🚧 | 🔗+🚧 | — | *no dedicated templates yet* |
| | Output | — | — | — | — | — | — | — | *no dedicated templates yet* |
| Event Hub | Trigger | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | — | Go |
| | Output | — | 🔗+🚧 | 🔗+🚧 | — | — | — | — | *no dedicated templates yet* |
| Event Grid | *(reserved)* | — | — | — | — | — | — | — | *no dedicated templates yet* |
| Queue | *(reserved)* | — | — | — | — | — | — | — | *no dedicated templates yet* |
| Service Bus | Trigger | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | — | Go |
| Cosmos DB | Trigger | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | — | Go |
| | Input | — | — | — | — | — | — | — | *no dedicated templates yet* |
| | Output | — | — | — | — | — | — | — | *no dedicated templates yet* |
| SQL | Trigger | ✅ | ✅ | ✅ | — | — | — | — | JS, Java, PS, Go |
| | Input | — | — | — | — | — | — | — | *no dedicated templates yet* |
| | Output | 🔗+🚧 | 🔗+🚧 | 🔗+🚧 | — | — | — | — | *no dedicated templates yet* |
| Redis | *(reserved)* | — | — | — | — | — | — | — | *no dedicated templates yet* |

### MCP templates (P50–P59)

| Sub-type | P | .NET | Py | TS | JS | Java | PS | Go | Gaps |
|---|---|---|---|---|---|---|---|---|---|
| Remote Server | P50 | ✅ | ✅ | ✅ | ✅ | ✅ | — | ✅ | PS |
| SDK Hosting | P51 | — | — | — | — | — | — | — | *removed (repos archived)* |
| Tool | P52 | 🔗 | 🔗 | 🔗 | — | 🔗 | — | — | *no dedicated templates yet* |
| Resource | P53 | 🔗 | 🔗 | 🔗 | — | 🔗 | — | — | *no dedicated templates yet* |
| Prompt | P54 | 🔗 | — | — | — | — | — | — | *no dedicated templates yet* |
| APIM Gateway | P55 | — | ✅ | — | — | — | — | — | .NET, TS, JS, Java, PS, Go |

### AI · Durable · Agent Framework (P60–P79)

| Category | Sub-type | P | .NET | Py | TS | JS | Java | PS | Go | Gaps |
|---|---|---|---|---|---|---|---|---|---|---|
| AI | Agent | P60 | ✅ | ✅ (2) | ✅ | — | ✅ | — | — | JS, PS, Go |
|  | ChatGPT | P61 | — | ✅ | — | ✅ | — | — | — | .NET, TS, Java, PS, Go |
|  | Text Summarize | P62 | — | — | — | — | — | — | — | *removed (repos archived)* |
|  | LangChain | P63 | — | ✅ | — | — | — | — | — | .NET, TS, JS, Java, PS, Go |
| Durable | Orchestration | P65 | ✅ | ✅ | ✅ | 🚧 | — | — | — | Java, PS, Go |
|  | Order Processor | P66 | ✅ | ✅ | — | — | — | — | — | TS, JS, Java, PS, Go |
|  | Patterns | P70 | ✅ | — | — | — | — | — | — | Py, TS, JS, Java, PS, Go |
|  | Scenarios | P71 | 🚧 | — | — | — | — | — | — | Py, TS, JS, Java, PS, Go |
|  | PDF Summarizer | P72 | 🚧 | 🚧 | — | — | — | — | — | TS, JS, Java, PS, Go |
| Agent Fw | Multi-Agent | P75 | — | 🚧 | — | — | — | — | ✅ | .NET, TS, JS, Java, PS |

### Connectors (P80–P89)

| Sub-type | P | .NET | Py | TS | JS | Java | PS | Go | Gaps |
|---|---|---|---|---|---|---|---|---|---|
| Office 365 Outlook | P80 | ✅ | ✅ | ✅ | — | — | — | — | JS, Java, PS, Go |
| SharePoint Online | P81 | ✅ | ✅ | ✅ | — | — | — | — | JS, Java, PS, Go |

## Summary

| P | Slot label | Templates |
|--:|---|--:|
| 00 | ⚡ HTTP — Trigger | 7 |
| 03 | ⚡ HTTP — Variant (Terraform) | 1 |
| 05 | ⏰ Timer — Trigger | 6 |
| 10 | 🪣 Blob Storage — Trigger | 6 |
| 15 | 📡 Event Hub — Trigger | 6 |
| 30 | 🚌 Service Bus — Trigger | 6 |
| 35 | 🌐 Cosmos DB — Trigger | 6 |
| 40 | 🗄️ SQL — Trigger | 3 |
| 50 | 🔌 MCP — Remote Server | 6 |
| 55 | 🔌 MCP — APIM Gateway | 1 |
| 60 | 🤖 AI — Agent | 5 |
| 61 | 🤖 AI — ChatGPT | 2 |
| 63 | 🤖 AI — LangChain | 1 |
| 65 | 🔄 Durable Standard — Orchestration | 4 |
| 66 | 🔄 Durable Standard — Order Processor | 2 |
| 70 | 🔄 Durable Advanced — Patterns (saga / tracing / payload) | 4 |
| 71 | 🔄 Durable Advanced — Scenarios (travel / aspire) | 2 |
| 72 | 🔄 Durable Advanced — PDF Summarizer | 2 |
| 75 | 🤝 Agent Framework — Multi-Agent | 2 |
| 80 | 🔌 Connectors — Office 365 Outlook | 3 |
| 81 | 🔌 Connectors — SharePoint Online | 3 |
| | **Total** | **78** |

## Language sort order

When templates share the same priority, sort by:

| Rank | Language | Rationale |
|---|---|---|
| 1 | .NET (C#) | Primary SDK, largest Azure Functions user base |
| 2 | Python | Fastest-growing, AI/ML use cases |
| 3 | TypeScript | Modern JS with types |
| 4 | JavaScript | Broad web developer reach |
| 5 | Java | Enterprise |
| 6 | PowerShell | IT automation / scripting |
| 7 | Go | Cloud-native services and tooling |

## Schema change required

```json
// manifest.schema.json — update priority field:
"priority": {
  "type": "integer",
  "minimum": 0,
  "maximum": 99,
  "description": "Priority for sorting. 5-slot blocks per Azure resource (+0 trigger, +1 input, +2 output, +3 variant). 10-slot block for MCP (+0 remote, +1 sdk, +2 tool, +3 resource, +4 prompt, +5 apim). See docs/priority-tiers.md."
}
```
