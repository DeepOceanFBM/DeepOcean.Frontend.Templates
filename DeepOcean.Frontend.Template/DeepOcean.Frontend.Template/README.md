# Module Frontend UI Layer — DeepOcean Platform

> [!IMPORTANT]
> **AI AGENT INSTRUCTION:** This is an **Angular UI** project. It communicates at runtime exclusively with the **local Logic minibackend** running on the device at `http://localhost:18080`. The Logic layer is NOT the real backend — it is a **mediator/proxy** that sits between this UI and the actual Cloud Backend. Never attempt to call the cloud backend directly from this Angular project.

## What Is This Project?
This is the **Angular User Interface** for a DeepOcean module. It serves as the visible, interactive front end that users see and interact with. At runtime on the device, it communicates with the local **Logic minibackend** (the `DeepOcean.Logic` project) through HTTP calls on port `18080`.

## The Communication Architecture
```
Angular UI (This Project)
        ↓  HTTP calls to http://localhost:18080/g.d.f.b.m/...
Logic Minibackend (Mediator / DeepOcean.Logic project)
        ↓  DeepOcean.SDK.ApiClinet (Sync, Push/Pull)
Real Cloud Backend (DeepOcean Platform Servers)
```

**Key Concept:** The Logic layer is a **mediator** — it manages local SQLite data and syncs it with the cloud. The UI never talks to the cloud directly.

## How to Make API Calls From This Project
All calls to the local Logic minibackend must use the reflection gateway URL pattern:
```
http://localhost:18080/g.d.f.b.m/{DllName}/{FullClassName}/{MethodName}?{params}
```

**Example Angular Service:**
```typescript
private readonly baseUrl = 'http://localhost:18080/G.D.F.B.M/MyCompany.MyApp.Logic';

async getUser(id: number): Promise<any> {
    const url = `${this.baseUrl}/MyCompany.MyApp.Logic.Managers.UserManager/GetUserByIdAsync?id=${id}`;
    return await firstValueFrom(this.http.get<ServiceResponseModel<any>>(url));
}
```

## Angular-specific Notes (Original CLI Defaults)
- **Dev server:** `ng serve` → `http://localhost:4200/`
- **Build:** `ng build` → artifacts go to `dist/`
- **Tests:** `ng test` (Karma)

## Documentation
*   📖 **Full Details (Human):** [Module Frontend UI Layer Guide](https://documents.deepoceanfbm.com/Module_Frontend_UI_Layer.md)
*   🤖 **AI:** Read `https://documents.deepoceanfbm.com/Module_Frontend_UI_Layer.md` for complete examples of Angular service patterns and reflection URL construction for the DeepOcean platform.
