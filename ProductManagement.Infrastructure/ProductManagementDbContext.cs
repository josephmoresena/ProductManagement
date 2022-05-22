
using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;

using ProductManagement.Entities;

namespace ProductManagement.Infrastructure
{
    public sealed class ProductManagementDbContext : DbContext
    {
        public DbSet<Product> Products => this.Set<Product>();
        [ExcludeFromCodeCoverage]
        public DbSet<ProductProvider> Providers => this.Set<ProductProvider>();

        public ProductManagementDbContext(DbContextOptions options) : base(options) { }
    }
}
