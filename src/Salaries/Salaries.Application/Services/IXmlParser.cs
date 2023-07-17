using Salaries.Application.Services.Models;

namespace Salaries.Application.Services;

public interface IXmlParser
{
    Dictionary<SheetCell, string> GetSheetCells(Stream fileStream, string sheetName);
    int GetSheetRowCount(Stream fileStream, string sheetName);
}