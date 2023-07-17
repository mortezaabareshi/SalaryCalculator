using OfficeOpenXml;
using Salaries.Application.Services;
using Salaries.Application.Services.Models;

namespace Salaries.Infrastructure.Parsers;

public class XmlParser : IXmlParser
{
    public Dictionary<SheetCell, string> GetSheetCells(Stream fileStream, string sheetName)
    {
#if DEBUG
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
#endif
        using var package = new ExcelPackage();
        package.Load(fileStream);
        return GetSheetCells(package, sheetName);
    }
    
    public int GetSheetRowCount(Stream fileStream, string sheetName)
    {
#if DEBUG
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
#endif
        using var package = new ExcelPackage();
        package.Load(fileStream);
        return GetSheetRowCount(package, sheetName);
    }

    private Dictionary<SheetCell, string> GetSheetCells(ExcelPackage package, string sheetName)
    {
        var worksheet = package.Workbook.Worksheets[sheetName];
        var cells = worksheet.Cells;
        var result = cells
            .GroupBy(c => new { c.Start.Row, c.Start.Column })
            .ToDictionary(
                g => new SheetCell
                {
                    Row = g.Key.Row,
                    Column = g.Key.Column
                },
                g => cells[g.Key.Row, g.Key.Column].Value?.ToString());

        return result;
    }
    
    private int GetSheetRowCount(ExcelPackage package, string sheetName)
    {
        var worksheet = package.Workbook.Worksheets[sheetName];
        var result = worksheet.Dimension.End.Row;

        return result;
    }
}