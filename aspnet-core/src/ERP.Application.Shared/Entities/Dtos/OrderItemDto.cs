using System;
using Abp.Application.Services.Dto;

namespace ERP.Entities.Dtos
{
    public class OrderItemDto : EntityDto
    {
        public int Quantity { get; set; }

        public int Price { get; set; }

        public int? OrderId { get; set; }

        public int BookId { get; set; }

    }
}