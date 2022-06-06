
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SourceCode.Dtos
{
    public class CreateOrEditGLSRCEDto : EntityDto<int?>
    {

		[Required]
		public string SRCELEDGER { get; set; }
		
		
		[Required]
		public string SRCETYPE { get; set; }
		
		
		[Required]
		public string SRCEDESC { get; set; }
		
		

    }
}