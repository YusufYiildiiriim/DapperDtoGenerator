namespace DapperDtoGenerator.Models;

public class DatabaseColumn
{
    public string ColumnName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public bool AllowDBNull { get; set; }
}