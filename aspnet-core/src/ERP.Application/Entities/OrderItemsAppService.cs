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
        private readonly IRepository<Book, int> _lookup_bookRepository;

        public OrderItemsAppService(IRepository<OrderItem> orderItemRepository, IRepository<Book, int> lookup_bookRepository)
        {
            _orderItemRepository = orderItemRepository;
            _lookup_bookRepository = lookup_bookRepository;

        }

        public async Task<PagedResultDto<GetOrderItemForViewDto>> GetAll(GetAllOrderItemsInput input)
        {

            var filteredOrderItems = _orderItemRepository.GetAll()
                        .Include(e => e.BookFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookTitleFilter), e => e.BookFk != null && e.BookFk.Title == input.BookTitleFilter);

            var pagedAndFilteredOrderItems = filteredOrderItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderItems = from o in pagedAndFilteredOrderItems
                             join o1 in _lookup_bookRepository.GetAll() on o.BookId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new
                             {

                                 o.Quantity,
                                 o.Price,
                                 Id = o.Id,
                                 BookTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString()
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