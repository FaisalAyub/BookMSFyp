using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Entities.Dtos;
using ERP.Dto;

namespace ERP.Entities
{
    public interface IBooksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBookForViewDto>> GetAll(GetAllBooksInput input);

        Task<GetBookForViewDto> GetBookForView(int id);

		Task<GetBookForEditOutput> GetBookForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBookDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBooksToExcel(GetAllBooksForExcelInput input);

		
		Task<PagedResultDto<BookUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}