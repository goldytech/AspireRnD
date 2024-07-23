namespace GraphQLApi.Data.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public CategoryEntity CategoryEntity { get; set; }

    // Timestamp for Concurrent Control
    public byte[] RowVersion { get; set; }

    // Last Modified TimeStamp
    public DateTime LastModifiedAt { get; set; } = DateTime.Now;
}