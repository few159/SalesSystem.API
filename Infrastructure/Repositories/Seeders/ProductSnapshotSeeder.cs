using MongoDB.Driver;
using SalesSystem.Application.Interfaces.Repositories.Stores;
using SalesSystem.Domain.Enitities.Snapshot;

namespace SalesSystem.Infrastructure.Repositories.Seeders;

public static class ProductSnapshotSeeder
{
    public static async Task SeedAsync(IMongoDbContext context)
    {
        var collection = context.ProductSnapshots;
        
        await collection.DeleteManyAsync(FilterDefinition<ProductSnapshot>.Empty);
        var products = new List<ProductSnapshot>
        {
            new() { Id = Guid.NewGuid(), Title = "Camisa Polo", Price = 89.90m, Description = "Camisa polo de algodão", Category = "Roupas Masculinas", Image = "https://example.com/camisa1.jpg" },
            new() { Id = Guid.NewGuid(), Title = "Calça Jeans", Price = 129.90m, Description = "Calça jeans slim fit", Category = "Roupas Masculinas", Image = "https://example.com/calca1.jpg" },
            new() { Id = Guid.NewGuid(), Title = "Tênis Esportivo", Price = 199.99m, Description = "Tênis leve e confortável", Category = "Calçados", Image = "https://example.com/tenis1.jpg" },
            new() { Id = Guid.NewGuid(), Title = "Relógio Digital", Price = 149.00m, Description = "Relógio à prova d'água", Category = "Acessórios", Image = "https://example.com/relogio1.jpg" },
            new() { Id = Guid.NewGuid(), Title = "Boné Estilizado", Price = 49.90m, Description = "Boné de aba curva", Category = "Acessórios", Image = "https://example.com/bone1.jpg" },
            new() { Id = Guid.NewGuid(), Title = "Notebook Gamer", Price = 5999.00m, Description = "Notebook com RTX 4060", Category = "Informática", Image = "https://example.com/notebook1.jpg" },
            new() { Id = Guid.NewGuid(), Title = "Smartphone 5G", Price = 2299.00m, Description = "Tela OLED 6.1\"", Category = "Eletrônicos", Image = "https://example.com/celular1.jpg" },
            new() { Id = Guid.NewGuid(), Title = "Fone Bluetooth", Price = 299.90m, Description = "Cancelamento de ruído ativo", Category = "Áudio", Image = "https://example.com/fone1.jpg" },
            new() { Id = Guid.NewGuid(), Title = "Mochila Executiva", Price = 159.90m, Description = "Resistente à água", Category = "Acessórios", Image = "https://example.com/mochila1.jpg" },
            new() { Id = Guid.NewGuid(), Title = "Echo Dot", Price = 349.00m, Description = "Assistente virtual Alexa", Category = "Casa Inteligente", Image = "https://example.com/echodot.jpg" },
        };

        await collection.InsertManyAsync(products);
    }
}