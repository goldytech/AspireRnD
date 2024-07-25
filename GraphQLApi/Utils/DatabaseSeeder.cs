using System.Diagnostics;
using GraphQLApi.Data.Entities;
using MongoDB.Driver;

namespace GraphQLApi.Utils;

public class DatabaseSeeder(IMongoDatabase database, ILogger<DatabaseSeeder> logger)
{
    private static readonly ActivitySource activitySource = new("DatabaseSeeder");

    public async Task SeedData()
    {
        using var activity = activitySource.StartActivity("SeedDatabase");
        {
            try
            {
                var productCollection = database.GetCollection<ProductEntity>("Products");
                var categoryCollection = database.GetCollection<CategoryEntity>("Categories");

                var productCount = await productCollection.CountDocumentsAsync(FilterDefinition<ProductEntity>.Empty);
                if (productCount == 0)
                {
                    // Assuming you have predefined categories and products
                    var defaultCategory = new CategoryEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = "Electronics",
                        Description = "A category for electronic devices",
                        LastModifiedAt = DateTime.UtcNow
                    };

                    await categoryCollection.InsertOneAsync(defaultCategory);

                    var defaultProducts = new List<ProductEntity>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Samsung Galaxy S21",
                            Description = "A flagship phone from Samsung",
                            Price = 1299.99M,
                            Category = defaultCategory,
                            LastModifiedAt = DateTime.UtcNow
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Apple iPhone 12",
                            Description = "A flagship phone from Apple",
                            Price = 19.99M,
                            Category = defaultCategory,
                            LastModifiedAt = DateTime.UtcNow
                        }
                    };

                    await productCollection.InsertManyAsync(defaultProducts);
                    logger.LogDatabaseSeedingSuccess();
                }
            }
            catch (Exception ex)
            {
                logger.LogDatabaseSeedingError(nameof(SeedData), ex);
                activity.SetExceptionTags(ex);
                throw;
            }
        }
    }
}