using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DrinkStorage.Application.Products.Responses;
using DrinkStorage.Persistence;
using DrinkStorage.Persistence.Products;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace DrinkStorage.Application.Products;

public class ProductSerivce
{
    private readonly ILogger<ProductSerivce> _logger;
    private readonly ProductRepository _repository;
    private IUnitOfWork _unitOfWork;

    public ProductSerivce(ILogger<ProductSerivce> logger, ProductRepository repository, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductInfo> GetProductInfoAsync(CancellationToken token)
    {
        var products = await _repository.FindAllAsync(token);
        return new ProductInfo(
            products,
            products.Select(p => p.Price).DefaultIfEmpty(0).Max());
    }

    public Task<List<Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken token) =>
        _repository.FindByIdsAsync(ids.ToList(), token);

    public Task<List<Product>> FindByBrandAndPrice(Guid? brandId, int maxPrice, CancellationToken token) =>
        _repository.FindByBrandAndPrice(brandId, maxPrice, token);

    public async Task ImportFromXlsx(Stream steam, CancellationToken token)
    {
        using SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(steam, false);
        WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart ?? throw new InvalidOperationException();
        SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
        SharedStringTable sst = sstpart.SharedStringTable;
        WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
        SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
        _logger.LogInformation("Import begin");
        const int batchSize = 1000;
        var products = new List<Product>(batchSize);
        int totalImported = 0;
        foreach (Row row in sheetData.Elements<Row>().Skip(1))
        {
            if (products.Count == batchSize)
            {
                int saved = await SaveProducts(products, token);
                totalImported += saved;
            }
            if (TryParseProduct(row, sst, out var product))
            {
                products.Add(product);
            }
            else
            {
                _logger.LogError("Can't parse product from row {}", row);
            }
        }
        if (products.Count > 0)
        {
            await SaveProducts(products, token);
        }
        _logger.LogInformation("Import completed, products saved: {}", totalImported);
    }

    private async Task<int> SaveProducts(List<Product> products, CancellationToken token)
    {
        try
        {
            await _repository.AddRangeAsync(products, token);
            int saved = await _unitOfWork.SaveChangesAsync(token);
            _logger.LogInformation("Import progress, imported in batch: {}", saved);
            return saved;
        }
        catch (Exception ex)
        {
            _logger.LogError("Import error: {}", ex);
            throw;
        }
    }

    private static string GetCellText(Cell cell, SharedStringTable sst)
    {
        if (cell.CellValue is null)
            return string.Empty;

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
        if (cells.Count != 5)
        {
            product = null;
            return false;
        }
        if (!Guid.TryParse(GetCellText(cells[4], sst), out Guid brandId))
        {
            product = null;
            return false;
        }
        if (!int.TryParse(GetCellText(cells[1], sst), out int price))
        {
            product = null;
            return false;
        }
        if (!int.TryParse(GetCellText(cells[2], sst), out int quantity))
        {
            product = null;
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
