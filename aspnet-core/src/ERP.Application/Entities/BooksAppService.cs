using ERP.Authorization.Users;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Entities.Exporting;
using ERP.Entities.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Runtime.Session;

namespace ERP.Entities
{
	[AbpAuthorize(AppPermissions.Pages_Books)]
    public class BooksAppService : ERPAppServiceBase, IBooksAppService
    {
		 private readonly IRepository<Book> _bookRepository;
		 private readonly IBooksExcelExporter _booksExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public BooksAppService(IRepository<Book> bookRepository, IBooksExcelExporter booksExcelExporter , IRepository<User, long> lookup_userRepository) 
		  {
			_bookRepository = bookRepository;
			_booksExcelExporter = booksExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetBookForViewDto>> GetAll(GetAllBooksInput input)
         {
			
			var filteredBooks = _bookRepository.GetAll()
						.Include( e => e.OwnerFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.ISBN.Contains(input.Filter) || e.Author.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Publisher.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title.ToLower() == input.TitleFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.ISBNFilter),  e => e.ISBN.ToLower() == input.ISBNFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.AuthorFilter),  e => e.Author.ToLower() == input.AuthorFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description.ToLower() == input.DescriptionFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.PublisherFilter),  e => e.Publisher.ToLower() == input.PublisherFilter.ToLower().Trim())
						.WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
						.WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
						.WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
						.WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.OwnerFk != null && e.OwnerFk.Name.ToLower() == input.UserNameFilter.ToLower().Trim());

			var pagedAndFilteredBooks = filteredBooks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var books = from o in pagedAndFilteredBooks
                         join o1 in _lookup_userRepository.GetAll() on o.OwnerId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBookForViewDto() {
							Book = new BookDto
							{
                                Title = o.Title,
                                ISBN = o.ISBN,
                                Author = o.Author,
                                Description = o.Description,
                                Publisher = o.Publisher,
                                Price = o.Price,
                                Quantity = o.Quantity,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredBooks.CountAsync();

            return new PagedResultDto<GetBookForViewDto>(
                totalCount,
                await books.ToListAsync()
            );
         }
		 
		 public async Task<GetBookForViewDto> GetBookForView(int id)
         {
            var book = await _bookRepository.GetAsync(id);

            var output = new GetBookForViewDto { Book = ObjectMapper.Map<BookDto>(book) };

		    if (output.Book.OwnerId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Book.OwnerId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Books_Edit)]
		 public async Task<GetBookForEditOutput> GetBookForEdit(EntityDto input)
         {
            var book = await _bookRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBookForEditOutput {Book = ObjectMapper.Map<CreateOrEditBookDto>(book)};

		    if (output.Book.OwnerId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Book.OwnerId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBookDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Books_Create)]
		 private async Task Create(CreateOrEditBookDto input)
         {
            var book = ObjectMapper.Map<Book>(input);
            book.OwnerId = AbpSession.GetUserId();



            await _bookRepository.InsertAsync(book);
         }

		 [AbpAuthorize(AppPermissions.Pages_Books_Edit)]
		 private async Task Update(CreateOrEditBookDto input)
         {
            var book = await _bookRepository.FirstOrDefaultAsync((int)input.Id);
            book.OwnerId = AbpSession.GetUserId();
            ObjectMapper.Map(input, book);
         }

		 [AbpAuthorize(AppPermissions.Pages_Books_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _bookRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBooksToExcel(GetAllBooksForExcelInput input)
         {
			
			var filteredBooks = _bookRepository.GetAll()
						.Include( e => e.OwnerFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.ISBN.Contains(input.Filter) || e.Author.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Publisher.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title.ToLower() == input.TitleFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.ISBNFilter),  e => e.ISBN.ToLower() == input.ISBNFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.AuthorFilter),  e => e.Author.ToLower() == input.AuthorFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description.ToLower() == input.DescriptionFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.PublisherFilter),  e => e.Publisher.ToLower() == input.PublisherFilter.ToLower().Trim())
						.WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
						.WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
						.WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
						.WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.OwnerFk != null && e.OwnerFk.Name.ToLower() == input.UserNameFilter.ToLower().Trim());

			var query = (from o in filteredBooks
                         join o1 in _lookup_userRepository.GetAll() on o.OwnerId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBookForViewDto() { 
							Book = new BookDto
							{
                                Title = o.Title,
                                ISBN = o.ISBN,
                                Author = o.Author,
                                Description = o.Description,
                                Publisher = o.Publisher,
                                Price = o.Price,
                                Quantity = o.Quantity,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						 });


            var bookListDtos = await query.ToListAsync();

            return _booksExcelExporter.ExportToFile(bookListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Books)]
         public async Task<PagedResultDto<BookUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BookUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new BookUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<BookUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}