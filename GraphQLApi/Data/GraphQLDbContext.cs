using GraphQLApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.Data;

public class GraphQlDbContext(DbContextOptions<GraphQlDbContext> options) : DbContext(options)
{
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CategoryEntityConfiguration());
        builder.ApplyConfiguration(new ProductEntityConfiguration());
    }
}