using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Dtos
{
    public class GetBookForEditOutput
    {
		public CreateOrEditBookDto Book { get; set; }

		public string UserName { get; set;}


    }
}