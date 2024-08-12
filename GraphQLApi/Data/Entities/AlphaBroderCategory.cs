using HotChocolate.Authorization;

namespace GraphQLApi.Data.Entities;
[Authorize(Policy = "AlphaTenantsPolicy")]
public class AlphaBroderCategory
{
    public string Id { get; set; }
    [GraphQLDescription("The name of the category.")]
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime LastModifiedAt { get; set; }
}