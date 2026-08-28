using DeepOcean.SDK.SchemaModel;

namespace DeepOcean.Schema.Template;

[DeepOceanTable(
    "Clients",
    Description = "Client master data"
)]
public class Clients
{
    [SchemaColumn(
        "Id",
        DeepOceanColumnType.Int,
        IsPrimaryKey = true,
        IsIdentity = true
    )]
    public int Id { get; set; }

    [SchemaColumn(
        "Name",
        DeepOceanColumnType.String,
        Length = 200,
        Description = "Client Name App",
        IsRequired = true
    )]
    public string Name { get; set; } = string.Empty;

    [SchemaColumn(
        "Phone",
        DeepOceanColumnType.String,
        Length = 55,
        Description = "Client Phone",
        IsRequired = false
    )]
    public string Phone { get; set; } = string.Empty;

 



}