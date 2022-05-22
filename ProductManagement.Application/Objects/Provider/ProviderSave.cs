using System;
using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Objects
{
    [ExcludeFromCodeCoverage]
    public record ProviderSave
    {
        public String Description { get; set; }
        public String Phone { get; set; }
    }
}
