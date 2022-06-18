using Abp.Application.Services.Dto;
using System;

namespace ERP.Entities.Dtos
{
    public class GetAllOrdersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxOrderDateFilter { get; set; }
        public DateTime? MinOrderDateFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string StatusFilter { get; set; }

        public int? MaxTotalBillFilter { get; set; }
        public int? MinTotalBillFilter { get; set; }

        public string OrderNameFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}