namespace GraphQLApi.Data.Entities;

[Node(IdField = nameof(ProductId))]
public class SanMarProduct
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductDetails { get; set; }
    public decimal Cost { get; set; }
    public SanMarCategory ProductCategory { get; set; }
    public DateTime ModifiedAt { get; set; }
    public int StockQuantity { get; set; }
    public bool IsAvailable { get; set; }
}