using Abp.Application.Services.Dto;
using System;

namespace ERP.Entities.Dtos
{
    public class GetAllOrderItemsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxQuantityFilter { get; set; }
        public int? MinQuantityFilter { get; set; }

        public int? MaxPriceFilter { get; set; }
        public int? MinPriceFilter { get; set; }

        public string BookTitleFilter { get; set; }

    }
}