using System;

namespace ProductManagement.Objects
{
    public sealed record PaginationSettings
    {
        public Int32 RowsByPage { get; set; }
    }
}
