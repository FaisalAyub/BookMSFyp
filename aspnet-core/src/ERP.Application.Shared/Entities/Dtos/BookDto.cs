
using System;
using Abp.Application.Services.Dto;

namespace ERP.Entities.Dtos
{
    public class BookDto : EntityDto
    {
		public string Title { get; set; }

		public string ISBN { get; set; }

		public string Author { get; set; }

		public string Description { get; set; }

		public string Publisher { get; set; }

		public int Price { get; set; }

		public int Quantity { get; set; }


		 public long OwnerId { get; set; }

		 
    }
}