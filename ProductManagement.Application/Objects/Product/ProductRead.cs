using System;

namespace ProductManagement.Objects
{
    public sealed record ProductRead : ProductSave
    {
        public Int32 Code { get; set; }
        public String ProviderDescription { get; set; }
        public String ProviderPhone { get; set; }
        public ProductStatus Status { get; set; }
    }
}