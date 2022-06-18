using ERP.Entities;

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
    [AbpAuthorize(AppPermissions.Pages_OrderItems)]
    public class OrderItemsAppService : ERPAppServiceBase, IOrderItemsAppService
    {
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Order, int> _lookup_orderRepository;
        private readonly IRepository<Book, int> _lookup_bookRepository;

        public OrderItemsAppService(IRepository<OrderItem> orderItemRepository, IRepository<Order, int> lookup_orderRepository, IRepository<Book, int> lookup_bookRepository)
        {
            _orderItemRepository = orderItemRepository;
            _lookup_orderRepository = lookup_orderRepository;
            _lookup_bookRepository = lookup_bookRepository;

        }

        public async Task<PagedResultDto<GetOrderItemForViewDto>> GetAll(GetAllOrderItemsInput input)
        {

            var filteredOrderItems = _orderItemRepository.GetAll()
                        .Include(e => e.OrderFk)
                        .Include(e => e.BookFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.OrderOrderNameFilter), e => e.OrderFk != null && e.OrderFk.OrderName == input.OrderOrderNameFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.BookTitleFilter), e => e.BookFk != null && e.BookFk.Title == input.BookTitleFilter)
                        .WhereIf(input.OrderIdFilter.HasValue, e => false || e.OrderId == input.OrderIdFilter.Value);

            var pagedAndFilteredOrderItems = filteredOrderItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderItems = from o in pagedAndFilteredOrderItems
                             join o1 in _lookup_orderRepository.GetAll() on o.OrderId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _lookup_bookRepository.GetAll() on o.BookId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new
                             {

                                 o.Quantity,
                                 o.Price,
                                 Id = o.Id,
                                 OrderOrderName = s1 == null || s1.OrderName == null ? "" : s1.OrderName.ToString(),
                                 BookTitle = s2 == null || s2.Title == null ? "" : s2.Title.ToString()
                             };

            var totalCount = await filteredOrderItems.CountAsync();

            var dbList = await orderItems.ToListAsync();
            var results = new List<GetOrderItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderItemForViewDto()
                {
                    OrderItem = new OrderItemDto
                    {

                        Quantity = o.Quantity,
                        Price = o.Price,
                        Id = o.Id,
                    },
                    OrderOrderName = o.OrderOrderName,
                    BookTitle = o.BookTitle
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderItemForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderItemForViewDto> GetOrderItemForView(int id)
        {
            var orderItem = await _orderItemRepository.GetAsync(id);

            var output = new GetOrderItemForViewDto { OrderItem = ObjectMapper.Map<OrderItemDto>(orderItem) };

            if (output.OrderItem.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((int)output.OrderItem.OrderId);
                output.OrderOrderName = _lookupOrder?.OrderName?.ToString();
            }

            if (output.OrderItem.BookId != null)
            {
                var _lookupBook = await _lookup_bookRepository.FirstOrDefaultAsync((int)output.OrderItem.BookId);
                output.BookTitle = _lookupBook?.Title?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderItems_Edit)]
        public async Task<GetOrderItemForEditOutput> GetOrderItemForEdit(EntityDto input)
        {
            var orderItem = await _orderItemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderItemForEditOutput { OrderItem = ObjectMapper.Map<CreateOrEditOrderItemDto>(orderItem) };

            if (output.OrderItem.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((int)output.OrderItem.OrderId);
                output.OrderOrderName = _lookupOrder?.OrderName?.ToString();
            }

            if (output.OrderItem.BookId != null)
            {
                var _lookupBook = await _lookup_bookRepository.FirstOrDefaultAsync((int)output.OrderItem.BookId);
                output.BookTitle = _lookupBook?.Title?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderItemDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrderItems_Create)]
        protected virtual async Task Create(CreateOrEditOrderItemDto input)
        {
            var orderItem = ObjectMapper.Map<OrderItem>(input);

            await _orderItemRepository.InsertAsync(orderItem);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderItems_Edit)]
        protected virtual async Task Update(CreateOrEditOrderItemDto input)
        {
            var orderItem = await _orderItemRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, orderItem);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderItems_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _orderItemRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_OrderItems)]
        public async Task<PagedResultDto<OrderItemOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.OrderName != null && e.OrderName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderItemOrderLookupTableDto>();
            foreach (var order in orderList)
            {
                lookupTableDtoList.Add(new OrderItemOrderLookupTableDto
                {
                    Id = order.Id,
                    DisplayName = order.OrderName?.ToString()
                });
            }

            return new PagedResultDto<OrderItemOrderLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_OrderItems)]
        public async Task<List<OrderItemBookLookupTableDto>> GetAllBookForTableDropdown()
        {
            return await _lookup_bookRepository.GetAll()
                .Select(book => new OrderItemBookLookupTableDto
                {
                    Id = book.Id,
                    DisplayName = book == null || book.Title == null ? "" : book.Title.ToString()
                }).ToListAsync();
        }

    }
}