using System.Diagnostics;
using System.Text.Json;
using GraphQLApi.Data.Entities;
using GraphQLApi.Utils;
using MongoDB.Driver;

namespace GraphQLApi.Data;

public class DatabaseSeeder
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<DatabaseSeeder> _logger;
    private static readonly ActivitySource ActivitySource = new("DatabaseSeeder");

    public DatabaseSeeder(IMongoDatabase database, ILogger<DatabaseSeeder> logger)
    {
        _database = database ?? throw new ArgumentNullException(nameof(database));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SeedData()
    {
        using var activity = ActivitySource.StartActivity("SeedDatabase");
        {
            try
            {
                var sanMarProductCollection = _database.GetCollection<SanMarProduct>("SanMarProducts");
                var sanMarCategoryCollection = _database.GetCollection<SanMarCategory>("SanMarCategories");
                var alphaBroderProductCollection = _database.GetCollection<AlphaBroderProduct>("AlphaBroderProducts");
                var alphaBroderCategoryCollection = _database.GetCollection<AlphaBroderCategory>("AlphaBroderCategories");

                if (sanMarProductCollection == null || sanMarCategoryCollection == null || alphaBroderProductCollection == null || alphaBroderCategoryCollection == null)
                {
                    _logger.LogError("Database collections are null.");
                }

                var sanMarProductCount = await sanMarProductCollection.CountDocumentsAsync(FilterDefinition<SanMarProduct>.Empty);
                var alphaBroderProductCount = await alphaBroderProductCollection.CountDocumentsAsync(FilterDefinition<AlphaBroderProduct>.Empty);

                var dataInserted = false;

                if (sanMarProductCount == 0)
                {
                    var sanMarProducts = await LoadDataFromFile<SanMarProduct>("./Data/SanMarProducts.json");
                    if (!sanMarProducts.Any())
                    {
                        _logger.LogError("SanMarProducts data is null or empty.");
                    }

                    var sanMarCategories = sanMarProducts.Select(p => p.ProductCategory).Distinct().ToList();
                    if (sanMarCategories == null || !sanMarCategories.Any())
                    {
                        throw new InvalidOperationException("SanMarCategories data is null or empty.");
                    }

                    await sanMarCategoryCollection?.InsertManyAsync(sanMarCategories)!;
                    await sanMarProductCollection.InsertManyAsync(sanMarProducts);
                    dataInserted = true;
                }

                if (alphaBroderProductCount == 0)
                {
                    var alphaBroderProducts = await LoadDataFromFile<AlphaBroderProduct>("./Data/AlphaBroderProducts.json");
                    if (alphaBroderProducts == null || !alphaBroderProducts.Any())
                    {
                        throw new InvalidOperationException("AlphaBroderProducts data is null or empty.");
                    }

                    var alphaBroderCategories = alphaBroderProducts.Select(p => p.AlphaBroderCategory).Distinct().ToList();
                    if (alphaBroderCategories == null || !alphaBroderCategories.Any())
                    {
                        throw new InvalidOperationException("AlphaBroderCategories data is null or empty.");
                    }

                    if (alphaBroderCategoryCollection is null)
                    {
                        _logger.LogError("AlphaBroderCategory collection is null.");
                    }
                    else
                    {
                        await alphaBroderCategoryCollection.InsertManyAsync(alphaBroderCategories);
                        await alphaBroderProductCollection.InsertManyAsync(alphaBroderProducts);
                        dataInserted = true;
                    }
                }

                if (dataInserted)
                {
                    _logger.LogDatabaseSeedingSuccess();
                }
                else
                {
                    _logger.LogDatabaseAlreadySeeded();
                }
            }
            catch (Exception ex)
            {
                _logger.LogDatabaseSeedingError(nameof(SeedData), ex);
                activity?.SetExceptionTags(ex);
                throw;
            }
        }
    }

    private static async Task<List<T>> LoadDataFromFile<T>(string filePath)
    {
        await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return await JsonSerializer.DeserializeAsync<List<T>>(stream) ?? new List<T>();
    }
}