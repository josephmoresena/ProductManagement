using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Entities
{
    [Table(nameof(Product), Schema = nameof(ProductManagement))]
    public class Product
    {
        public Int32 Id { get; set; }
        [Required, MaxLength(100)]
        public String Description { get; set; }
        public Boolean Active { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Int32 ProviderId { get; set; }
        public virtual ProductProvider Provider { get; protected set; }

        public Product()
        {
            this.Active = true;
        }
    }
}
