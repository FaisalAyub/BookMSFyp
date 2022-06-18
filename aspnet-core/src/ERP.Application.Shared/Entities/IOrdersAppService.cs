using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Entities.Dtos;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.Entities
{
    public interface IOrdersAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderForViewDto>> GetAll(GetAllOrdersInput input);

        Task<GetOrderForViewDto> GetOrderForView(int id);

        Task<GetOrderForEditOutput> GetOrderForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOrderDto input);

        Task Delete(EntityDto input);

        Task<List<OrderUserLookupTableDto>> GetAllUserForTableDropdown();

    }
}