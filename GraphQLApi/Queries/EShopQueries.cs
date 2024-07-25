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
    public  IExecutable<ProductEntity>? GetProducts()
    {
        var productCollection = database.GetCollection<ProductEntity>("Products");
        return productCollection.AsExecutable();
    }
    
    [UseFirstOrDefault]
    public IExecutable<ProductEntity> GetProductById(Guid id)
    {
        var productCollection = database.GetCollection<ProductEntity>("Products");
        return productCollection.Find(x=>x.Id == id).AsExecutable();
    }
}