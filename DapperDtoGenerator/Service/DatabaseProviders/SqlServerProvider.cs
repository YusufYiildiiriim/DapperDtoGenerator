using DapperDtoGenerator.Models;
using DapperDtoGenerator.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DapperDtoGenerator.Services.DatabaseProviders;

public class SqlServerProvider : IDatabaseService
{
    public async Task<IEnumerable<DatabaseColumn>> GetSchemaAsync(string connectionString, string sqlQuery)
    {
        var columns = new List<DatabaseColumn>();
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sqlQuery, connection);
        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync(CommandBehavior.SchemaOnly);
        var schemaTable = reader.GetSchemaTable();
        if (schemaTable != null)
        {
            foreach (DataRow row in schemaTable.Rows)
            {
                var dataType = (Type)row["DataType"];

                columns.Add(new DatabaseColumn
                {
                    ColumnName = row["ColumnName"].ToString() ?? "Unknown",
                    DataType = dataType.Name,
                    AllowDBNull = (bool)row["AllowDBNull"]
                });
            }
        }

        return columns;
    }
}
