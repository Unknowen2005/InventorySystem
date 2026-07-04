using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Verifica se já há dados
        if (await context.Categorias.AnyAsync())
            return;

        // Cria categorias padrão
        var categorias = new List<Categoria>
        {
            new("Eletrônicos", "Produtos eletrônicos em geral"),
            new("Escritório", "Materiais de escritório"),
            new("Perecíveis", "Produtos com prazo de validade curto"),
            new("Vestuário", "Roupas e acessórios")
        };

        await context.Categorias.AddRangeAsync(categorias);

        // Cria usuário administrador padrão (senha: Admin@123)
        var admin = new Usuario(
            "Administrador",
            "admin@sistema.com",
            HashPassword("Admin@123"),
            PerfilAcesso.Administrador
        );

        await context.Usuarios.AddAsync(admin);

        await context.SaveChangesAsync();
    }

    // Método simples de hash (use BCrypt em produção!)
    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}