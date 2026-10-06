# AI Coding Guidelines for AviationSystem

1. **Architecture**: We use .NET 10 Minimal APIs. DO NOT generate MVC Controllers.
2. **Entities**: All domain models must inherit from `BaseEntity`. 
3. **Database**: Use Entity Framework Core 10. Entity configurations must be placed in the `Data/Configurations` folder using `IEntityTypeConfiguration`.