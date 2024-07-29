using GraphQLApi.Data.Entities;
using GraphQLApi.Utils;
using HotChocolate.Data;
using MongoDB.Driver;

namespace GraphQLApi.Queries;


public class EShopQueries(IMongoDatabase database, ILogger<EShopQueries> logger)
{
    [UsePaging]
    [UseProjection]
    [UseSorting]
    [UseFiltering]
    public  IExecutable<AlphaBroderProduct>? GetAlphaBroderProducts()
    {
        try
        {
            var productCollection = database.GetCollection<AlphaBroderProduct>("AlphaBroderProducts");
            return productCollection.AsExecutable();
        }
        catch (Exception e)
        {
            logger.LogGraphQlApiError();
            return null;
        }
       
    }
    
    [UseFirstOrDefault]
    public IExecutable<AlphaBroderProduct> GetAlphaBroderProductById(string id)
    {
        var productCollection = database.GetCollection<AlphaBroderProduct>("AlphaBroderProducts");
        return productCollection.Find(x=>x.Id == id).AsExecutable();
    }
    
    [UsePaging]
    [UseProjection]
    [UseSorting]
    [UseFiltering]
    public IExecutable<SanMarProduct>? GetSanMarProducts()
    {
        try
        {
            var productCollection = database.GetCollection<SanMarProduct>("SanMarProducts");
            return productCollection.AsExecutable();
        }
        catch (Exception e)
        {
            logger.LogGraphQlApiError();
            return null;
        }
        
    }
    
    [UseFirstOrDefault]
    public IExecutable<SanMarProduct> GetSanMarProductById(string id)
    {
        var productCollection = database.GetCollection<SanMarProduct>("SanMarProducts");
        return productCollection.Find(x=>x.Id == id).AsExecutable();
    }
}