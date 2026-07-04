dotnet add src/Inventory.Domain/Inventory.Domain.csproj reference src/Inventory.Shared/Inventory.Shared.csproj
dotnet add src/Inventory.Application/Inventory.Application.csproj reference src/Inventory.Domain/Inventory.Domain.csproj
dotnet add src/Inventory.Application/Inventory.Application.csproj reference src/Inventory.Shared/Inventory.Shared.csproj
dotnet add src/Inventory.Infrastructure/Inventory.Infrastructure.csproj reference src/Inventory.Application/Inventory.Application.csproj
dotnet add src/Inventory.Infrastructure/Inventory.Infrastructure.csproj reference src/Inventory.Shared/Inventory.Shared.csproj
dotnet add src/Inventory.WPF/Inventory.WPF.csproj reference src/Inventory.Application/Inventory.Application.csproj
dotnet add src/Inventory.WPF/Inventory.WPF.csproj reference src/Inventory.Infrastructure/Inventory.Infrastructure.csproj
pause