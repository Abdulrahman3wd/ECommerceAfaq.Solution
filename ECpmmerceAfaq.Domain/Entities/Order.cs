using ECommerceAfaq.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; } = null!; 

        public string ShippingFullname { get; set; } = null!;
        public string ShippingPhone { get; set; } = null!;
        public string ShippingCity { get; set;} = null!;
        public string ShippingArea { get; set;} = null!;
        public string ShippingStreet { get; set; } = null!;
        public string? ShippingBuilding { get; set;} 
        public string? ShippingAppartment { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public PaymentMethod PaymentMethod { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;


        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
