using System;

namespace ProductManagement.Objects
{
    public record ProductSave
    {
        public String Description { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Int32 ProviderCode { get; set; }
    }
}