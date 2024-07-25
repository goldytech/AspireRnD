namespace GraphQLApi.Data.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public CategoryEntity Category { get; set; }
    public DateTime LastModifiedAt { get; set; }
}