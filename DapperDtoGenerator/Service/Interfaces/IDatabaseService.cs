using DapperDtoGenerator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperDtoGenerator.Services.Interfaces;

public interface IDatabaseService
{
    Task<IEnumerable<DatabaseColumn>> GetSchemaAsync(string connectionString, string sqlQuery);
}