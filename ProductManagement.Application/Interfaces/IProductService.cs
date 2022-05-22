using ProductManagement.Objects;

namespace ProductManagement.Interfaces
{
    public interface IProductService : IProductService<ProductRead, ProductSave, ProductFilter>
    {
    }
}
