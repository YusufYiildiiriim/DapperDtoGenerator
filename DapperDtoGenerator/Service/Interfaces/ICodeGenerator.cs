using DapperDtoGenerator.Models;
using System.Collections.Generic;

namespace DapperDtoGenerator.Services.Interfaces;

public interface ICodeGenerator
{
    string GenerateClass(string className, IEnumerable<DatabaseColumn> columns);
    string GeneratePropertiesOnly(IEnumerable<DatabaseColumn> columns);
}