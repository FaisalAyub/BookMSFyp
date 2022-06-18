using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Entities.Dtos;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.Entities
{
    public interface IOrderItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderItemForViewDto>> GetAll(GetAllOrderItemsInput input);

        Task<GetOrderItemForViewDto> GetOrderItemForView(int id);

        Task<GetOrderItemForEditOutput> GetOrderItemForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOrderItemDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<OrderItemOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input);

        Task<List<OrderItemBookLookupTableDto>> GetAllBookForTableDropdown();

    }
}