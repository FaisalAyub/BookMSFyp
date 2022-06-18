using ERP.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Entities.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;

namespace ERP.Entities
{
    [AbpAuthorize(AppPermissions.Pages_Orders)]
    public class OrdersAppService : ERPAppServiceBase, IOrdersAppService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public OrdersAppService(IRepository<Order> orderRepository, IRepository<User, long> lookup_userRepository)
        {
            _orderRepository = orderRepository;
            _lookup_userRepository = lookup_userRepository;

        }

        public async Task<PagedResultDto<GetOrderForViewDto>> GetAll(GetAllOrdersInput input)
        {

            var filteredOrders = _orderRepository.GetAll()
                        .Include(e => e.OrderByFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter) || e.Status.Contains(input.Filter) || e.OrderName.Contains(input.Filter))
                        .WhereIf(input.MinOrderDateFilter != null, e => e.OrderDate >= input.MinOrderDateFilter)
                        .WhereIf(input.MaxOrderDateFilter != null, e => e.OrderDate <= input.MaxOrderDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusFilter), e => e.Status == input.StatusFilter)
                        .WhereIf(input.MinTotalBillFilter != null, e => e.TotalBill >= input.MinTotalBillFilter)
                        .WhereIf(input.MaxTotalBillFilter != null, e => e.TotalBill <= input.MaxTotalBillFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderNameFilter), e => e.OrderName == input.OrderNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.OrderByFk != null && e.OrderByFk.Name == input.UserNameFilter);

            var pagedAndFilteredOrders = filteredOrders
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orders = from o in pagedAndFilteredOrders
                         join o1 in _lookup_userRepository.GetAll() on o.OrderBy equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new
                         {

                             o.OrderDate,
                             o.Description,
                             o.Status,
                             o.TotalBill,
                             o.OrderName,
                             Id = o.Id,
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         };

            var totalCount = await filteredOrders.CountAsync();

            var dbList = await orders.ToListAsync();
            var results = new List<GetOrderForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderForViewDto()
                {
                    Order = new OrderDto
                    {

                        OrderDate = o.OrderDate,
                        Description = o.Description,
                        Status = o.Status,
                        TotalBill = o.TotalBill,
                        OrderName = o.OrderName,
                        Id = o.Id,
                    },
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderForViewDto> GetOrderForView(int id)
        {
            var order = await _orderRepository.GetAsync(id);

            var output = new GetOrderForViewDto { Order = ObjectMapper.Map<OrderDto>(order) };

            if (output.Order.OrderBy != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Order.OrderBy);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Orders_Edit)]
        public async Task<GetOrderForEditOutput> GetOrderForEdit(EntityDto input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderForEditOutput { Order = ObjectMapper.Map<CreateOrEditOrderDto>(order) };

            if (output.Order.OrderBy != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Order.OrderBy);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Orders_Create)]
        protected virtual async Task Create(CreateOrEditOrderDto input)
        {
            var order = ObjectMapper.Map<Order>(input);

            if (AbpSession.TenantId != null)
            {
                order.TenantId = (int?)AbpSession.TenantId;
            }
            order.OrderBy = AbpSession.UserId;
            order.OrderDate = new DateTime();
            order.Status = "Create Order"; 
          int orderId=  await _orderRepository.InsertAndGetIdAsync(order);
            if(orderId >0)
            {

            }

        }

        [AbpAuthorize(AppPermissions.Pages_Orders_Edit)]
        protected virtual async Task Update(CreateOrEditOrderDto input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, order);

        }

        [AbpAuthorize(AppPermissions.Pages_Orders_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _orderRepository.DeleteAsync(input.Id);
        }
        [AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<List<OrderUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new OrderUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

    }
}