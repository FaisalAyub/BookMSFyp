using Abp.Application.Services.Dto;

namespace ERP.AccountGroup.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}