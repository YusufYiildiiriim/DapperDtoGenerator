using DapperDtoGenerator.Models;
using DapperDtoGenerator.Services.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace DapperDtoGenerator.Services.CodeGenerators;

public class CSharpCodeGenerator : ICodeGenerator
{
    public string GenerateClass(string className, IEnumerable<DatabaseColumn> columns)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"public class {className}");
        sb.AppendLine("{");

        sb.Append(GeneratePropertiesOnly(columns));

        sb.AppendLine("}");
        return sb.ToString();
    }

    public string GeneratePropertiesOnly(IEnumerable<DatabaseColumn> columns)
    {
        var sb = new StringBuilder();
        foreach (var col in columns)
        {
            var csharpType = MapDotNetTypeToCSharpKeyword(col.DataType);

            var isNullable = col.AllowDBNull && IsValueType(csharpType) ? "?" : "";


            sb.AppendLine($"    public {csharpType}{isNullable} {col.ColumnName} {{ get; set; }}");
        }
        return sb.ToString();
    }

    private string MapDotNetTypeToCSharpKeyword(string typeName)
    {
        return typeName switch
        {
            "Int16" => "short",
            "Int32" => "int",
            "Int64" => "long",
            "String" => "string",
            "Boolean" => "bool",
            "DateTime" => "DateTime",
            "Decimal" => "decimal",
            "Double" => "double",
            "Single" => "float",
            "Guid" => "Guid",
            "Byte[]" => "byte[]",
            _ => typeName
        };
    }

    private bool IsValueType(string typeName)
    {
        return typeName is not "string" and not "byte[]";
    }
}
