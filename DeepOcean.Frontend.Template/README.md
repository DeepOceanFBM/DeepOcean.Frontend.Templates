# Deep Ocean Module Starter Template

## What is this?
This folder contains the **Starter Template** for creating new modules on the **DeepOcean ERP Platform**. When a developer or AI assistant needs to create a new application (module) from scratch, this template serves as the foundation.

It provides a ready-to-run, minimal application skeleton that integrates seamlessly with the DeepOcean hybrid architecture (MAUI + EmbedIO + Angular + SQLite).

## Template Structure
This template is divided into three main projects/folders to enforce clean architecture:

1. **`DeepOcean.Frontend.Template`** 
   * **Role:** The user interface (UI) built with Angular. 
   * **What to do:** Contains the static views, components, and client-side services. This will be compiled and hosted by the local server.

2. **`DeepOcean.Logic.Template`**
   * **Role:** The C# Backend business logic. 
   * **What to do:** Contains the API controllers, business workflows, and handlers. All endpoints here are accessed via reflection from the frontend.

3. **`DeepOcean.Schema.Template`**
   * **Role:** The **Cloud Backend (SQL Server) schema definition**. This is NOT the local SQLite database.
   * **What to do:** Contains only C# classes that define the shape of the tables on the DeepOcean Cloud Backend. The Platform reads these classes and manages the actual database creation and migrations on the cloud SQL Server automatically. The developer only writes the class — no SQL, no migrations, no backend code.

## How to Use This Template
To build a new module based on this template:

1. **Duplicate the Template:** Copy this entire directory to a new location.
2. **Rename the Projects:** Rename the folders and `.csproj` files from `*.Template` to match your project's unique `PackageId` (e.g., `MyCompany.MyApp.MyLoc`).
3. **Update Namespaces:** Do a global find-and-replace in your code editor to change the `DeepOcean.*.Template` namespace to your new namespace.
4. **Develop:** Start writing your specific application logic, UI, and database schema.
5. **Upload & Deploy:** Use the `deepoceancli publish` command to upload your module's files. After the upload completes, you must log in to the DeepOcean Console at [https://console.deepoceanfbm.com/](https://console.deepoceanfbm.com/) to manually deploy your database schema and create the final release. For a complete step-by-step guide on the publishing process, refer to the [Quick Start Guide](https://documents.deepoceanfbm.com/#/docs/Quick_Start).

## AI Instructions
If you are an **AI Coding Assistant**:
* Do **NOT** modify the files in this template directly to solve a user's task unless explicitly asked to "update the template itself".
* If asked to create a *new* module, copy these files to the target directory and refactor the namespaces accordingly.
* **CRITICAL:** Do NOT attempt to configure, use, or reference `EmbedIO` or `minibackend` in the `Logic.Template`. The frontend communicates with the backend via standard HTTP calls, and the core platform handles all routing automatically. The C# backend only needs to expose standard methods returning `ServiceResponseModel`.
