# Module Schema Layer — DeepOcean Platform

> [!IMPORTANT]
> **AI AGENT INSTRUCTION:** This project defines the **Cloud SQL Server Backend schema only**. It is NOT for SQLite. The developer writes simple C# classes here, and the DeepOcean Platform automatically creates and manages the corresponding tables on the Cloud SQL Server. Do NOT write migrations, SQL queries, or any backend logic here.

## What Is This Project?
This project defines the **shape of the data tables** on the DeepOcean **Cloud Backend (SQL Server)**. It is the contract between the developer and the cloud platform.

The DeepOcean Platform is designed to **abstract away all backend complexity**. The developer's only responsibility is to write a C# class that describes what the table should look like. The platform handles everything else:
- Creating the table on the Cloud SQL Server.
- Managing schema migrations when columns are added or changed.
- Making the data accessible to the Logic Mediator layer for sync operations.

## What Goes Here
✅ C# classes that define the column structure of a cloud table (like a model/DTO).  
✅ Data annotations for column types, max lengths, keys, etc. (if supported).  
❌ No SQL queries or raw SQL.  
❌ No migrations code.  
❌ No business logic or HTTP endpoints.  
❌ No SQLite-specific code (`[PrimaryKey]`, `[AutoIncrement]` are for the Logic layer, not here).  

## Example Schema Class
```csharp
namespace MyCompany.MyApp.Schema
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
```
That's it. The platform reads this class and creates the corresponding table on the Cloud SQL Server automatically.

## How It Relates to Local SQLite
The **local SQLite** database (used for offline-first operation) is managed by the **Logic Mediator layer** (`DeepOcean.Logic`), NOT this project. The Logic layer typically mirrors the same data shape from here but adds SQLite-specific attributes like `[PrimaryKey, AutoIncrement] public int SQLite_ID`.

```
DeepOcean.Schema (This Project)     →  Cloud SQL Server (Online)
DeepOcean.Logic / CoreInitializer   →  Local SQLite (Offline)
```

## Documentation
*   📖 **Full Details (Human):** [Module Schema Layer Guide](https://documents.deepoceanfbm.com/Module_Schema_Layer.md)
*   🤖 **AI:** Fetch `https://documents.deepoceanfbm.com/Module_Schema_Layer.md` for the complete guide on writing Cloud Backend schema classes for the DeepOcean platform.
