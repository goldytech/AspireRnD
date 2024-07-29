using GraphQLApi.Data.Entities;
using HotChocolate.Data;
using MongoDB.Driver;

namespace GraphQLApi.Queries;


public class EShopQueries(IMongoDatabase database)
{
    [UsePaging]
    [UseProjection]
    [UseSorting]
    [UseFiltering]
    public  IExecutable<AlphaBroderProduct>? GetProducts()
    {
        var productCollection = database.GetCollection<AlphaBroderProduct>("Products");
        return productCollection.AsExecutable();
    }
    
    [UseFirstOrDefault]
    public IExecutable<AlphaBroderProduct> GetProductById(string id)
    {
        var productCollection = database.GetCollection<AlphaBroderProduct>("Products");
        return productCollection.Find(x=>x.Id == id).AsExecutable();
    }
}