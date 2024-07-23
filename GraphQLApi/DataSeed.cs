using GraphQLApi.Data;
using GraphQLApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace GraphQLApi;

public class DataSeed : IDbSeeder<GraphQlDbContext>
{
    public async Task SeedAsync(GraphQlDbContext context)
    {
        await context.Database.OpenConnectionAsync();
        await ((NpgsqlConnection)context.Database.GetDbConnection()).ReloadTypesAsync();
        if (!context.Categories.Any() || !context.Products.Any()) SeedCategories(context);

        await context.SaveChangesAsync();
    }


    private void SeedCategories(GraphQlDbContext context)
    {
        var electronicsCategoryId = Guid.NewGuid();
        var clothingCategoryId = Guid.NewGuid();
        var booksCategoryId = Guid.NewGuid();

        var categories = new List<CategoryEntity>
        {
            new()
            {
                Id = electronicsCategoryId,
                Name = "Electronics",
                Description = "Electronic items",
                Products = new List<ProductEntity>
                {
                    new()
                    {
                        Name = "Laptop", Description = "A portable computer", Price = 1200,
                        LastModifiedAt = DateTime.Now
                    },
                    new()
                    {
                        Name = "Smartphone", Description = "A personal device", Price = 800,
                        LastModifiedAt = DateTime.Now
                    }
                }
            },
            new()
            {
                Id = clothingCategoryId,
                Name = "Clothing",
                Description = "Clothing items",
                Products = new List<ProductEntity>
                {
                    new() { Name = "T-Shirt", Description = "A casual top", Price = 20, LastModifiedAt = DateTime.Now },
                    new() { Name = "Jeans", Description = "Denim pants", Price = 50, LastModifiedAt = DateTime.Now }
                }
            },
            new()
            {
                Id = booksCategoryId,
                Name = "Books",
                Description = "Books",
                Products = new List<ProductEntity>
                {
                    new()
                    {
                        Name = "The Hitchhiker's Guide to the Galaxy", Description = "A science fiction comedy",
                        Price = 15, LastModifiedAt = DateTime.Now
                    },
                    new()
                    {
                        Name = "1984", Description = "A dystopian social science fiction novel", Price = 10,
                        LastModifiedAt = DateTime.Now
                    }
                }
            }
        };

        context.Categories.AddRange(categories);
    }
}

public interface IDbSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(TContext context);
}