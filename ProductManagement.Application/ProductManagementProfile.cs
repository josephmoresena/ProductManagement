using AutoMapper;

using ProductManagement.Entities;
using ProductManagement.Objects;

namespace ProductManagement
{
    public sealed class ProductManagementProfile : Profile
    {
        public ProductManagementProfile()
        {
            this.ProviderMap();
            this.ProductMap();
        }

        private void ProviderMap()
        {
            this.CreateMap<ProviderSave, ProductProvider>();
        }

        private void ProductMap()
        {
            this.CreateMap<ProductSave, Product>()
                .ForMember(p => p.ProviderId, m => m.MapFrom(s => s.ProviderCode));
            this.CreateMap<Product, ProductRead>()
                .ForMember(r => r.Code, m => m.MapFrom(p => p.Id))
                .ForMember(r => r.Status, m => m.MapFrom(p => p.Active ? ProductStatus.Active : ProductStatus.Inactive))
                .ForMember(r => r.ProviderCode, m => m.MapFrom(p => p.Provider.Id))
                .ForMember(r => r.ProviderDescription, m => m.MapFrom(p => p.Provider.Description))
                .ForMember(r => r.ProviderPhone, m => m.MapFrom(p => p.Provider.Phone));
        }
    }
}
