using System;

namespace Fairweather.Service
{
    public delegate void Allocator(decimal amount, bool amount_drained, bool outstanding_covered);
}
