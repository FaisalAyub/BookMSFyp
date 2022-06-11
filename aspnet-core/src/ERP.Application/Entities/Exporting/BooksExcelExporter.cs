using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Entities.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Entities.Exporting
{
    public class BooksExcelExporter : EpPlusExcelExporterBase, IBooksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BooksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBookForViewDto> books)
        {
            return CreateExcelPackage(
                "Books.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Books"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Title"),
                        L("ISBN"),
                        L("Author"),
                        L("Description"),
                        L("Publisher"),
                        L("Price"),
                        L("Quantity"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, books,
                        _ => _.Book.Title,
                        _ => _.Book.ISBN,
                        _ => _.Book.Author,
                        _ => _.Book.Description,
                        _ => _.Book.Publisher,
                        _ => _.Book.Price,
                        _ => _.Book.Quantity,
                        _ => _.UserName
                        );

					

                });
        }
    }
}
