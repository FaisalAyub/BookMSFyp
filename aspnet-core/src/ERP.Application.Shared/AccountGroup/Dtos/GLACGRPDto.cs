
using System;
using Abp.Application.Services.Dto;

namespace ERP.AccountGroup.Dtos
{
    public class GLACGRPDto : EntityDto<int>
    {
		public int Id { get; set; }
        public string ACCTGRPCOD { get; set; }

        public string ACCTGRPDES { get; set; }

		public string SORTCODE { get; set; }

		public short GRPCOD { get; set; }



    }
}