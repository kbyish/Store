

using Store.Contracts;
using Store.Models;

namespace Store.Contracts;
public class CsvFileData
{
    public int NumberOfDataLines { get; set; }
    public List<CreateProductDto> Products { get; set; } = [];
}  