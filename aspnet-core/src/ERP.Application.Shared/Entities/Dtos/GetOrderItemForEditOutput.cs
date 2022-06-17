using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Dtos
{
    public class GetOrderItemForEditOutput
    {
        public CreateOrEditOrderItemDto OrderItem { get; set; }

        public string BookTitle { get; set; }

    }
}