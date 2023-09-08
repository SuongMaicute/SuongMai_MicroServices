﻿using Suongmai.Services.OrderAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suongmai.Services.OrderAPI.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader? OrderHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
