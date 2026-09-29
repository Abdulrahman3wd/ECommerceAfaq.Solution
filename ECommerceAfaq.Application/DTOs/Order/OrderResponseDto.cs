using ECommerceAfaq.Domain.Entities;
using ECommerceAfaq.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Order
{
    public class OrderResponseDto
    {

        public int Id { get; set; }
        public List<OrderItemResponseDto> OrderItems { get; set; } = new();

        public string ShippingFullname { get; set; } = null!;
        public string ShippingPhone { get; set; } = null!;
        public string ShippingCity { get; set; } = null!;
        public string ShippingArea { get; set; } = null!;
        public string ShippingStreet { get; set; } = null!;
        public string? ShippingBuilding { get; set; }
        public string? ShippingAppartment { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus OrderStatus { get; set; } 

        public PaymentMethod PaymentMethod { get; set; }

        public PaymentStatus PaymentStatus { get; set; } 

        public DateTime CreateAt { get; set; }



    }
}
