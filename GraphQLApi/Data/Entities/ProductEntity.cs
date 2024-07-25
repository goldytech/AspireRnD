namespace GraphQLApi.Data.Entities;

[GraphQLDescription("The product entity.")]
[Node(IdField = nameof(Id))]
public class ProductEntity 
{
    public Guid Id { get; set; }
    [GraphQLDescription("The name of the product.")]
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public CategoryEntity Category { get; set; }
    public DateTime LastModifiedAt { get; set; }
}