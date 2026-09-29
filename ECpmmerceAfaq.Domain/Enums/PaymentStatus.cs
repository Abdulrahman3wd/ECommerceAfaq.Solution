using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending = 0,
        PendingVervication = 1,
        Paid = 2,
        Rejected = 3,
        Failed = 4
    }
}
