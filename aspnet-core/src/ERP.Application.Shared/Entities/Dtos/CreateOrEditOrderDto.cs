using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Dtos
{
    public class CreateOrEditOrderDto : EntityDto<int?>
    {

        public DateTime OrderDate { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public int TotalBill { get; set; }

        public string OrderName { get; set; }

        public long? OrderBy { get; set; }

    }
}