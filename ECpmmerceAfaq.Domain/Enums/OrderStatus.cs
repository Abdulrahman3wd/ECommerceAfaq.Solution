using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Proccessing = 2,
        Shipped = 3,
        Delevered = 4,
        Canceled = 5,
    }
}
