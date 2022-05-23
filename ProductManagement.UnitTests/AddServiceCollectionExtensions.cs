using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using AutoFixture;

using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using ProductManagement.Entities;
using ProductManagement.Infrastructure;
using ProductManagement.Infrastructure.Repositories;
using ProductManagement.Interfaces;
using ProductManagement.Objects;
using ProductManagement.Services;

using Xunit;

namespace ProductManagement.UnitTests
{
    [ExcludeFromCodeCoverage]
    public sealed class AddServiceCollectionExtensions
    {
        private static readonly PropertyInfo providerPropertyInfo = typeof(Product).GetRuntimeProperty(nameof(Product.Provider));
        private static readonly Fixture fixture = new();

        [Fact]
        internal void ApplicationTest()
        {
            Mock<IServiceCollection> services = new();
            services.Setup(s => s.Add(It.IsAny<ServiceDescriptor>()))
                .Callback<ServiceDescriptor>(sd =>
                {
                    Type serviceType = sd.ServiceType;
                    if (sd.Lifetime.Equals(ServiceLifetime.Singleton))
                    {
                        Assert.Equal(typeof(IMapper), serviceType);
                        IMapper mapper = sd.ImplementationInstance as IMapper;
                        Assert.NotNull(mapper);

                        ProductSave save = fixture.Create<ProductSave>();
                        Product product = mapper.Map<Product>(save);

                        Assert.Equal(save.Description, product.Description);
                        Assert.Equal(save.ManufacturingDate, product.ManufacturingDate);
                        Assert.Equal(save.ExpirationDate, product.ExpirationDate);
                        Assert.Equal(save.ProviderCode, product.ProviderId);
                        Assert.Equal(0, product.Id);
                        Assert.True(product.Active);
                        Assert.Null(product.Provider);
                        SetProvider(product);

                        ProductRead read = mapper.Map<ProductRead>(product);

                        Assert.Equal(save.Description, read.Description);
                        Assert.Equal(save.ManufacturingDate, read.ManufacturingDate);
                        Assert.Equal(save.ExpirationDate, read.ExpirationDate);
                        Assert.Equal(save.ProviderCode, read.ProviderCode);
                        Assert.Equal(product.Id, read.Code);
                        Assert.Equal(product.Provider.Description, read.ProviderDescription);
                        Assert.Equal(product.Provider.Phone, read.ProviderPhone);
                    }
                    else
                    {
                        if (serviceType.Equals(typeof(IProductService)))
                            Assert.Equal(typeof(ProductService), sd.ImplementationType);
                        else
                            Assert.Equal(typeof(ProductProviderService), sd.ImplementationType);
                    }
                });
            services.Object.AddApplication();
            services.Verify(s => s.Add(It.IsAny<ServiceDescriptor>()), Times.Exactly(3));
        }

        [Fact]
        internal void InfrastructureTest()
        {
            Mock<IServiceCollection> services = new();
            services.Setup(s => s.Add(It.IsAny<ServiceDescriptor>()))
                .Callback<ServiceDescriptor>(sd =>
                {
                    Type serviceType = sd.ServiceType;
                    Assert.Equal(ServiceLifetime.Scoped, sd.Lifetime);
                    if (serviceType.Equals(typeof(IProductRepository)))
                        Assert.Equal(typeof(ProductRepository), sd.ImplementationType);
                    else
                        Assert.Equal(typeof(ProductProviderRepostory), sd.ImplementationType);
                });
            services.Object.AddInfrastructure();
            services.Verify(s => s.Add(It.IsAny<ServiceDescriptor>()), Times.Exactly(2));
        }

        private static void SetProvider(Product product)
        {
            ProductProvider provider = fixture.Build<ProductProvider>()
                .With(p => p.Id, product.ProviderId).Create();
            providerPropertyInfo.SetValue(product, provider);
            product.Id = fixture.Create<Int32>();
        }
    }
}
