namespace GraphQLApi.Data.Entities;

public class CategoryEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<ProductEntity> Products { get; set; }

    // Timestamp for Concurrent Control
    public byte[] RowVersion { get; set; }

    // Last Modified TimeStamp
    public DateTime LastModifiedAt { get; set; } = DateTime.Now;
}