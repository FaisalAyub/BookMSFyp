using Abp.Application.Services.Dto;
using System;

namespace ERP.SourceCode.Dtos
{
    public class GetAllGLSRCEForExcelInput
    {
		public string Filter { get; set; }

		public string SRCELEDGERFilter { get; set; }

		public string SRCETYPEFilter { get; set; }

		public string SRCEDESCFilter { get; set; }



    }
}