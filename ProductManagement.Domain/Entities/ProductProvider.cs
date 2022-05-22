using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("Provider", Schema = nameof(ProductManagement))]
    public class ProductProvider
    {
        public Int32 Id { get; set; }
        [Required, MaxLength(100)]
        public String Description { get; set; }
        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public String Phone { get; set; }

        public virtual ISet<Product> Products { get; protected set; }

        public ProductProvider()
        {
            this.Products = new HashSet<Product>();
        }
    }
}
