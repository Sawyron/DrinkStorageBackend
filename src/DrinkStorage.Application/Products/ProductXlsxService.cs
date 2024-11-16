using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DrinkStorage.Persistence.Products;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace DrinkStorage.Application.Products;

public class ProductXlsxService
{
    private readonly ILogger<ProductXlsxService> _logger;

    public ProductXlsxService(ILogger<ProductXlsxService> logger)
    {
        _logger = logger;
    }

    public IEnumerable<Product> ParseFromXlsx(Stream stream)
    {
        using SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(stream, false);
        WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart ?? throw new InvalidOperationException();
        SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
        SharedStringTable sst = sstpart.SharedStringTable;
        WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
        SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
        foreach (Row row in sheetData.Elements<Row>().Skip(1))
        {
            if (TryParseProduct(row, sst, out Product? product))
            {
                yield return product;
            }
            else
            {
                _logger.LogWarning("Failed to parse product from row {}", row);
            }
        }
    }

    private static string GetCellText(Cell cell, SharedStringTable sst)
    {
        if (cell.CellValue is null)
        {
            return string.Empty;
        }
        if ((cell.DataType is not null) &&
            (cell.DataType == CellValues.SharedString))
        {
            int ssid = int.Parse(cell.CellValue.Text);
            return sst.ChildElements[ssid].InnerText;
        }
        return cell.CellValue.Text;
    }

    private static bool TryParseProduct(Row row, SharedStringTable sst, [NotNullWhen(true)] out Product? product)
    {
        List<Cell> cells = row.Elements<Cell>().ToList();
        product = null;
        if (cells.Count != 5)
        {
            return false;
        }
        if (!Guid.TryParse(GetCellText(cells[4], sst), out Guid brandId))
        {
            return false;
        }
        if (!int.TryParse(GetCellText(cells[1], sst), out int price))
        {
            return false;
        }
        if (!int.TryParse(GetCellText(cells[2], sst), out int quantity))
        {
            return false;
        }
        product = new Product
        {
            Name = GetCellText(cells[0], sst),
            Price = price,
            Quantity = quantity,
            BrandId = brandId,
            ImageUrl = GetCellText(cells[3], sst)
        };
        return true;
    }
}
