using System;
using Abp.Application.Services.Dto;

namespace ERP.Entities.Dtos
{
    public class OrderDto : EntityDto
    {
        public DateTime OrderDate { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public int TotalBill { get; set; }

        public string OrderName { get; set; }

        public long? OrderBy { get; set; }

    }
}