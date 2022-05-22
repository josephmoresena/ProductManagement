using System;

namespace ProductManagement.Objects
{
    public sealed record ProductFilter
    {
        private readonly Int32[] _codes;
        private readonly Int32[] _providers;

        public Int32[] Products
        {
            get => this._codes;
            init
            {
                if (value is not null)
                    this._codes = value;
            }
        }
        public Int32[] Providers
        {
            get => this._providers;
            init
            {
                if (value is not null)
                    this._providers = value;
            }
        }
        public String ProductDescription { get; init; }
        public Boolean ExactMatchProductDescription { get; init; }
        public String ProviderDescription { get; init; }
        public Boolean ExactMatchProviderDescription { get; init; }
        public ProductStatus? Status { get; set; }

        public ProductFilter()
        {
            this._codes = Array.Empty<Int32>();
            this._providers = Array.Empty<Int32>();
        }
    }
}
