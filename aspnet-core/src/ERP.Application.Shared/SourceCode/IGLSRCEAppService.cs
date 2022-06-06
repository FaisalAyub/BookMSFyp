using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SourceCode.Dtos;
using ERP.Dto;

namespace ERP.SourceCode
{
    public interface IGLSRCEAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGLSRCEForViewDto>> GetAll(GetAllGLSRCEInput input);

        Task<GetGLSRCEForViewDto> GetGLSRCEForView(int id);

		Task<GetGLSRCEForEditOutput> GetGLSRCEForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGLSRCEDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetGLSRCEToExcel(GetAllGLSRCEForExcelInput input);

		
    }
}