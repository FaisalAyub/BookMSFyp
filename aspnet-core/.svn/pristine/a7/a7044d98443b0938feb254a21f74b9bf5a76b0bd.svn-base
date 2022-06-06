using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.AccountGroup.Dtos;
using ERP.Dto;

namespace ERP.AccountGroup
{
    public interface IGLACGRPAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGLACGRPForViewDto>> GetAll(GetAllGLACGRPInput input);

        Task<GetGLACGRPForViewDto> GetGLACGRPForView(int id);

		Task<GetGLACGRPForEditOutput> GetGLACGRPForEdit(EntityDto<int> input);

		Task CreateOrEdit(CreateOrEditGLACGRPDto input);

		Task Delete(EntityDto<int> input);

		Task<FileDto> GetGLACGRPToExcel(GetAllGLACGRPForExcelInput input);

		
    }
}