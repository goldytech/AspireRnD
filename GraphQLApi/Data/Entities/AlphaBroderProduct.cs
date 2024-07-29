namespace GraphQLApi.Data.Entities;

[GraphQLDescription("The product entity of AlphaBroder.")]
[Node(IdField = nameof(Id))]
public class AlphaBroderProduct 
{
    public string Id { get; set; }
    [GraphQLDescription("The name of the product.")]
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public AlphaBroderCategory AlphaBroderCategory { get; set; }
    public DateTime LastModifiedAt { get; set; }
}