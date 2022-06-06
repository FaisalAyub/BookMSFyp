using Abp.Application.Services.Dto;
using System;

namespace ERP.AccountGroup.Dtos
{
    public class GetAllGLACGRPForExcelInput
    {
		public string Filter { get; set; }

		public string ACCTGRPCODFilter { get; set; }

		public string ACCTGRPDESFilter { get; set; }

		public string SORTCODEFilter { get; set; }

		public short? MaxGRPCODFilter { get; set; }
		public short? MinGRPCODFilter { get; set; }



    }
}