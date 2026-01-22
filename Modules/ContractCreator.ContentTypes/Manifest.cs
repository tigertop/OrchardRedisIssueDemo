using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "ContractCreator.ContentTypes",
    Author = "TopSoft",
    Website = "https://topsoft.am",
    Version = "0.0.1",
    Description = "ContractCreator.ContentTypes",
    Dependencies = new[] { "OrchardCore.Contents" },
    Category = "Content Management"
)]

[assembly: Feature(
    Id = "OrchardCore.Redis.DataProtection-Fixed",
    Name = "OrchardCore.Redis.DataProtection-Fixed",
    Description = "OrchardCore.Redis.DataProtection-Fixed",
    Dependencies = ["OrchardCore.Redis"],
    Category = "Distributed"
)]