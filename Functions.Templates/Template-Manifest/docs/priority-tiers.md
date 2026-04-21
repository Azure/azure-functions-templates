# Template Manifest — Priority Tiers (P00–P99)

**Total templates:** 76  
**Manifest version:** 1.4.0

## Design principles

1. **Priority is the primary sort key.** Lower number = show first.
2. **5-slot blocks per Azure resource:** `+0` trigger, `+1` input, `+2` output, `+3` variant, `+4` stub.
3. **10-slot block for MCP:** `+0` Remote Server, `+1` SDK Hosting, `+2` Tool, `+3` Resource, `+4` Prompt, `+5` APIM Gateway.
4. **Language sort order (secondary):** .NET (C#) → Python → TypeScript → JavaScript → Java → PowerShell.
   Templates sharing a priority are sorted by this language order by the consumer.
5. **IaC sort order (tertiary):** ❌ no-IaC → ✅ AZD → 📦 IaC-only.
   no-IaC = what VS Code Azure Functions extension generates = the familiar baseline.
6. **Default rule:** New templates without AZD/IaC default to P95–P99.

## Priority block map

| Range | Category | Supported bindings | Slot assignments |
|---|---|---|---|
| P00–04 | ⚡ HTTP | Trigger · Output | `P00` Trigger<br>`P01` Input *(N/A)*<br>`P02` Output *(future)*<br>`P03` Variant (Terraform)<br>`P04` Stub |
| P05–09 | ⏰ Timer | Trigger only | `P05` Trigger |
| P10–14 | 📦 Blob Storage | Trigger · Input · Output | `P10` Trigger<br>`P11` Input *(future)*<br>`P12` Output *(future)* |
| P15–19 | 📡 Event Hub | Trigger · Output | `P15` Trigger<br>`P16` Output *(future)* |
| P20–24 | 📣 Event Grid *(reserved)* | Trigger · Output | `P20` Trigger<br>`P21` Output |
| P25–29 | 📬 Queue Storage *(reserved)* | Trigger · Output | `P25` Trigger<br>`P26` Output |
| P30–34 | 🚌 Service Bus | Trigger · Output | `P30` Trigger<br>`P31` Output |
| P35–39 | 🌐 Cosmos DB | Trigger · Input · Output | `P35` Trigger<br>`P36` Input *(future)*<br>`P37` Output *(future)* |
| P40–44 | 🗄️ SQL | Trigger · Input · Output | `P40` Trigger<br>`P41` Input *(future)*<br>`P42` Output *(future)* |
| P45–49 | 🔴 Redis *(reserved)* | Trigger · Input · Output | `P45` Trigger<br>`P46` Input<br>`P47` Output |
| P50–59 | 🔌 MCP *(10-slot block)* | Trigger (Tool / Resource / Prompt) | `P50` Remote Server<br>`P51` SDK Hosting<br>`P52` Tool *(future)*<br>`P53` Resource *(future)*<br>`P54` Prompt *(future)*<br>`P55` APIM Gateway<br>`P56` *(reserved)*<br>`P57` *(reserved)*<br>`P58` *(reserved)*<br>`P59` *(reserved)* |
| P60–64 | 🤖 AI | — | `P60` Agent<br>`P61` ChatGPT<br>`P62` Text Summarize<br>`P63` LangChain |
| P65–69 | 🔄 Durable Standard | Orchestration | `P65` Orchestration<br>`P66` Order Processor |
| P70–74 | 🔄 Durable Advanced | Orchestration | `P70` Patterns (saga / tracing / payload)<br>`P71` Scenarios (travel / aspire)<br>`P72` PDF Summarizer |
| P75–79 | 🤝 Agent Framework | Orchestration | `P75` Multi-Agent |
| P80–89 | — *(reserved for future categories)* | — | — |
| P90–94 | 🏗️ IaC | — | `P90` Flex Consumption (ARM / Bicep / TF) |
| P95–99 | ⚠️ Non-AZD Stubs | — | `P95` Gap-fillers (AZD upgrade planned) |

## Full template listing

Sorted by priority → language order (.NET → Py → TS → JS → Java → PS).

| P | Template ID | Lang | Binding | IaC | Was |
|--:|---|---|---|---|---|
| | **⚡ HTTP** | | | | |
| 0 | `http-trigger-csharp-azd` | .NET | trigger | ✅ bicep | P0 |
| 0 | `http-trigger-python-azd` | Py | trigger | ✅ bicep | P0 |
| 0 | `http-trigger-typescript-azd` | TS | trigger | ✅ bicep | P0 |
| 0 | `http-trigger-javascript-azd` | JS | trigger | ✅ bicep | P0 |
| 0 | `http-trigger-java-azd` | Java | trigger | ✅ bicep | P0 |
| 0 | `http-trigger-powershell-azd` | PS | trigger | ✅ bicep | P0 |
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
| | **🗄️ SQL** | | | | |
| 40 | `sql-trigger-csharp-azd` | .NET | trigger | ✅ bicep | P1→40 |
| 40 | `sql-trigger-python-azd` | Py | trigger | ✅ bicep | P1→40 |
| 40 | `sql-trigger-typescript-azd` | TS | trigger | ✅ bicep | P1→40 |
| | **🔌 MCP *(10-slot block)*** | | | | |
| 50 | `mcp-server-remote-csharp` | .NET | trigger | 📦 bicep | P0→50 |
| 50 | `mcp-server-remote-python` | Py | trigger | 📦 bicep | P0→50 |
| 50 | `mcp-server-remote-typescript` | TS | trigger | 📦 bicep | P0→50 |
| 50 | `mcp-server-remote-java` | Java | trigger | 📦 bicep | P0→50 |
| 51 | `mcp-sdk-hosting-csharp` | .NET | trigger | 📦 bicep | P1→51 |
| 51 | `mcp-sdk-hosting-python` | Py | trigger | 📦 bicep | P1→51 |
| 51 | `mcp-sdk-hosting-typescript` | TS | trigger | 📦 bicep | P1→51 |
| 51 | `mcp-sdk-hosting-java` | Java | trigger | 📦 bicep | P1→51 |
| 55 | `mcp-server-apim-python` | Py | trigger | 📦 bicep | P2→55 |
| | **🤖 AI** | | | | |
| 60 | `ai-agent-csharp` | .NET | trigger | 📦 bicep | P0→60 |
| 60 | `ai-agent-python` | Py | trigger | 📦 bicep | P0→60 |
| 60 | `ai-agent-typescript` | TS | trigger | 📦 bicep | P0→60 |
| 60 | `ai-agent-java` | Java | trigger | 📦 bicep | P0→60 |
| 61 | `ai-chatgpt-python` | Py | trigger | 📦 bicep | P1→61 |
| 61 | `ai-chatgpt-javascript` | JS | trigger | 📦 bicep | P1→61 |
| 62 | `ai-textsummarize-csharp` | .NET | trigger | 📦 bicep | P2→62 |
| 62 | `ai-textsummarize-python` | Py | trigger | 📦 bicep | P2→62 |
| 63 | `ai-langchain-python` | Py | trigger | 📦 bicep | P2→63 |
| | **🔄 Durable Standard** | | | | |
| 65 | `durable-orchestration-csharp` | .NET | orchestration | ❌ none | P1→65 |
| 65 | `durable-orchestration-python` | Py | orchestration | ❌ none | P1→65 |
| 65 | `durable-orchestration-typescript` | TS | orchestration | ❌ none | P1→65 |
| 65 | `durable-orchestration-javascript` | JS | orchestration | ❌ none | P1→65 |
| 65 | `durable-orchestration-java` | Java | orchestration | ❌ none | P1→65 |
| 66 | `durable-order-processor-csharp` | .NET | orchestration | 📦 bicep | P1→66 |
| 66 | `durable-order-processor-python` | Py | orchestration | 📦 bicep | P1→66 |
| | **🔄 Durable Advanced** | | | | |
| 70 | `durable-distributed-tracing-csharp` | .NET | orchestration | ❌ none | P1→70 |
| 70 | `durable-large-payload-csharp` | .NET | orchestration | ❌ none | P1→70 |
| 70 | `durable-saga-csharp` | .NET | orchestration | ❌ none | P1→70 |
| 71 | `durable-ai-travel-planner-csharp` | .NET | orchestration | ❌ none | P2→71 |
| 71 | `durable-aspire-csharp` | .NET | orchestration | ❌ none | P2→71 |
| 72 | `durable-pdf-summarizer-csharp` | .NET | orchestration | ❌ none | P2→72 |
| 72 | `durable-pdf-summarizer-python` | Py | orchestration | ❌ none | P2→72 |
| | **🤝 Agent Framework** | | | | |
| 75 | `agentframework-durable-multi-agent-python` | Py | orchestration | ❌ none | P1→75 |
| | **🏗️ IaC** | | | | |
| 90 | `iac-flex-consumption-arm` | ARM | none | 📦 arm | P2→90 |
| 90 | `iac-flex-consumption-bicep` | Bicep | none | 📦 bicep | P2→90 |
| 90 | `iac-flex-consumption-terraform-azapi` | TF | none | 📦 terraform | P2→90 |
| 90 | `iac-flex-consumption-terraform-azurerm` | TF | none | 📦 terraform | P2→90 |
| | **⚠️ Non-AZD Stubs** | | | | |
| 95 | `cosmos-input-java` | Java | input | ❌ none | P1→95 |
| 95 | `cosmos-output-java` | Java | output | ❌ none | P1→95 |
| 95 | `cosmos-trigger-java` | Java | trigger | ❌ none | P1→95 |
| 95 | `eventhub-trigger-java` | Java | trigger | ❌ none | P0→95 |

## Coverage matrix

### Azure resource templates (P00–P44)

| Resource | Binding | .NET | Py | TS | JS | Java | PS | Gaps |
|---|---|---|---|---|---|---|---|---|
| HTTP | Trigger | 📦 | ✅ | ✅ | ✅ | ✅ | ✅ | — |
| Timer | Trigger | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | — |
| Blob | Trigger | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | — |
| | Input | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
| | Output | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
|  | Input | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
|  | Output | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
| Event Hub | Trigger | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ | JS, Java, PS |
| | Output | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
|  | Output | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
| Event Grid | *(reserved)* | — | — | — | — | — | — | no templates yet |
| Queue | *(reserved)* | — | — | — | — | — | — | no templates yet |
| Service Bus | Trigger | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | — |
| Cosmos DB | Trigger | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ | JS, Java, PS |
| | Input | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
| | Output | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
|  | Input | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
|  | Output | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
| SQL | Trigger | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ | JS, Java, PS |
| | Input | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
| | Output | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
|  | Input | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
|  | Output | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | *no templates yet* |
| Redis | *(reserved)* | — | — | — | — | — | — | no templates yet |

### MCP templates (P50–P59)

| Sub-type | P | .NET | Py | TS | JS | Java | PS | Gaps |
|---|---|---|---|---|---|---|---|---|
| Remote Server | P50 | 📦 | 📦 | 📦 | ❌ | 📦 | ❌ | JS, PS |
| SDK Hosting | P51 | 📦 | 📦 | 📦 | ❌ | 📦 | ❌ | JS, PS |
| Tool *(future)* | P52 | — | — | — | — | — | — | *not yet created* |
| Resource *(future)* | P53 | — | — | — | — | — | — | *not yet created* |
| Prompt *(future)* | P54 | — | — | — | — | — | — | *not yet created* |
| APIM Gateway | P55 | ❌ | 📦 | ❌ | ❌ | ❌ | ❌ | .NET, TS, JS, Java, PS |

### AI · Durable · Agent Framework (P60–P79)

| Category | Sub-type | P | .NET | Py | TS | JS | Java | PS | Gaps |
|---|---|---|---|---|---|---|---|---|---|
| AI | Agent | P60 | 📦 | 📦 | 📦 | ❌ | 📦 | ❌ | JS, PS |
|  | ChatGPT | P61 | ❌ | 📦 | ❌ | 📦 | ❌ | ❌ | .NET, TS, Java, PS |
|  | Text Summarize | P62 | 📦 | 📦 | ❌ | ❌ | ❌ | ❌ | TS, JS, Java, PS |
|  | LangChain | P63 | ❌ | 📦 | ❌ | ❌ | ❌ | ❌ | .NET, TS, JS, Java, PS |
| Durable | Orchestration | P65 | ⚠️ | ⚠️ | ⚠️ | ⚠️ | ⚠️ | ❌ | PS |
|  | Order Processor | P66 | 📦 | 📦 | ❌ | ❌ | ❌ | ❌ | TS, JS, Java, PS |
|  | Patterns | P70 | ⚠️ | ❌ | ❌ | ❌ | ❌ | ❌ | Py, TS, JS, Java, PS |
|  | Advanced | P71 | ⚠️ | ❌ | ❌ | ❌ | ❌ | ❌ | Py, TS, JS, Java, PS |
|  | PDF Summarizer | P72 | ⚠️ | ⚠️ | ❌ | ❌ | ❌ | ❌ | TS, JS, Java, PS |
| Agent Fw | Multi-Agent | P75 | ❌ | ⚠️ | ❌ | ❌ | ❌ | ❌ | .NET, TS, JS, Java, PS |

## Summary

| P | Slot label | Templates |
|--:|---|--:|
| 00 | ⚡ HTTP — Trigger | 6 |
| 03 | ⚡ HTTP — Variant (Terraform) | 1 |
| 05 | ⏰ Timer — Trigger | 6 |
| 10 | 📦 Blob Storage — Trigger | 6 |
| 15 | 📡 Event Hub — Trigger | 3 |
| 30 | 🚌 Service Bus — Trigger | 6 |
| 35 | 🌐 Cosmos DB — Trigger | 3 |
| 40 | 🗄️ SQL — Trigger | 3 |
| 50 | 🔌 MCP — Remote Server | 4 |
| 51 | 🔌 MCP — SDK Hosting | 4 |
| 55 | 🔌 MCP — APIM Gateway | 1 |
| 60 | 🤖 AI — Agent | 4 |
| 61 | 🤖 AI — ChatGPT | 2 |
| 62 | 🤖 AI — Text Summarize | 2 |
| 63 | 🤖 AI — LangChain | 1 |
| 65 | 🔄 Durable Standard — Orchestration | 5 |
| 66 | 🔄 Durable Standard — Order Processor | 2 |
| 70 | 🔄 Durable Advanced — Patterns (saga / tracing / payload) | 3 |
| 71 | 🔄 Durable Advanced — Scenarios (travel / aspire) | 2 |
| 72 | 🔄 Durable Advanced — PDF Summarizer | 2 |
| 75 | 🤝 Agent Framework — Multi-Agent | 1 |
| 90 | 🏗️ IaC — Flex Consumption (ARM / Bicep / TF) | 4 |
| 95 | ⚠️ Non-AZD Stubs — Gap-fillers (AZD upgrade planned) | 4 |
| | **Total** | **76** |

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
___BEGIN___COMMAND_DONE_MARKER___0
