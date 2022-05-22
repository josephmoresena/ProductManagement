using System;

namespace ProductManagement.Objects
{
    public sealed record LimitSettings
    {
        public Int32 Page { get; init; }
        public Int32 Limit { get; init; }
    }
}
