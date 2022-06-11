using Abp.Application.Services.Dto;
using System;

namespace ERP.Entities.Dtos
{
    public class GetAllBooksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TitleFilter { get; set; }

		public string ISBNFilter { get; set; }

		public string AuthorFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public string PublisherFilter { get; set; }

		public int? MaxPriceFilter { get; set; }
		public int? MinPriceFilter { get; set; }

		public int? MaxQuantityFilter { get; set; }
		public int? MinQuantityFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 
    }
}