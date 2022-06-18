using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Dtos
{
    public class CreateOrEditOrderItemDto : EntityDto<int?>
    {

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int? OrderId { get; set; }

        public int BookId { get; set; }

    }
}