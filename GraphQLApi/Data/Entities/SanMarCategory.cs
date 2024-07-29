namespace GraphQLApi.Data.Entities;

public class SanMarCategory
{
    public string CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategoryDescription { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}