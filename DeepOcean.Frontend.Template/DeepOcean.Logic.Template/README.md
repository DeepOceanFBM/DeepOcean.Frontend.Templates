# Module Logic Mediator Layer — DeepOcean Platform

> [!IMPORTANT]
> **AI AGENT INSTRUCTION:** This is a **C# Mediator/Minibackend** — NOT the real backend. It acts as an intermediary layer that runs locally on the device. It receives HTTP calls from the Angular UI, handles local SQLite data, and syncs with the actual cloud backend using the `DeepOcean.SDK.ApiClinet` package. Do NOT configure any web server (EmbedIO or otherwise) in this project. The core platform handles routing automatically.

## What Is This Project?
This is the **Logic (Minibackend/Mediator)** layer of a DeepOcean module. It is a C# Class Library that:
1.  Exposes C# methods that are invoked by the Angular UI via the reflection gateway at `http://localhost:18080/g.d.f.b.m/...`
2.  Manages local SQLite data (read/write) for offline-first operation.
3.  Uses the `DeepOcean.SDK.ApiClinet` package to synchronize local data with the cloud backend (Push/Pull).

## Role in the Architecture
```
Angular UI
    ↓  HTTP via Reflection Gateway (port 18080)
THIS PROJECT (Logic Mediator) ← You are here
    ↓  DeepOcean.SDK.ApiClinet
Real Cloud Backend
```

## Key NuGet Packages
*   **`DeepOcean.SDK.ApiClinet`** — The SDK that manages communication with the real cloud backend. Contains extension methods for Push/Pull data sync. **This is the only approved way to connect to the cloud backend.**
*   **`Newtonsoft.Json`** — Used to manually deserialize `string` JSON body parameters inside controller methods.
*   **`sqlite-net-pcl`** — Local SQLite database operations.
*   **`DeepOcean.Core.MK`, `DeepOcean.PM.MK`, `DeepOcean.CM.MK`** — Platform SDKs providing `ServiceResponseModel`, `CoreInitializer` helpers, and base runtime services.

> **SDK Source:** The `DeepOcean.SDK.ApiClinet` source code is available locally at `SDK/Client/DeepOcean.SDK.ApiClinet` for reference.

## Strict C# Method Signature Rules
All methods exposed to the Angular UI must follow these rules:
1.  **Return type** must always be `ServiceResponseModel<T>`.
2.  **All parameters** must be `string`.
3.  If the request has a POST body, the **first parameter** receives it as a raw JSON string.
4.  Additional query string parameters follow in **exact positional order** (not by name).

```csharp
public ServiceResponseModel<object> GetUser(string jsonBody, string queryParam1)
{
    var user = JsonConvert.DeserializeObject<UserDTO>(jsonBody);
    int id = int.Parse(queryParam1);
    return new ServiceResponseModel<object> { data = user, Success = true };
}
```

## Documentation
*   📖 **Full Details (Human):** [Module Logic Mediator Layer Guide](https://documents.deepoceanfbm.com/Module_Logic_Mediator_Layer.md)
*   🤖 **AI:** Read `https://documents.deepoceanfbm.com/Module_Logic_Mediator_Layer.md` for complete examples of C# controllers, SDK usage, and sync patterns in the DeepOcean platform.
