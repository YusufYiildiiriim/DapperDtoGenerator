using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DapperDtoGenerator.Services.CodeGenerators;
using DapperDtoGenerator.Services.DatabaseProviders;
using DapperDtoGenerator.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DapperDtoGenerator.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly ICodeGenerator _codeGenerator;

    public ObservableCollection<string> DatabaseTypes { get; } = new()
    {
        "SqlServer",
        "PostgreSQL",
        "MySQL",
        "SQLite"
    };

    [ObservableProperty]
    private string selectedDatabase = "SqlServer";

    [ObservableProperty]
    private string connectionString;

    [ObservableProperty]
    private string generateType;

    [ObservableProperty]
    private string result;

    [ObservableProperty]
    private string sqlQuery;

    [ObservableProperty]
    private bool isGenerating;

    public MainWindowViewModel()
    {
        _codeGenerator = new CSharpCodeGenerator();
    }

    [RelayCommand]
    private async Task GenerateAsync()
    {
        if (string.IsNullOrWhiteSpace(ConnectionString) || string.IsNullOrWhiteSpace(SqlQuery))
        {
            Result = "// Hata: Lütfen Connection String ve SQL Query alanlarını doldurun.";
            return;
        }

        IsGenerating = true;
        Result = "// Veritabanına bağlanılıyor ve DTO oluşturuluyor...";

        try
        {
            IDatabaseService dbService = GetDatabaseService(SelectedDatabase);

            var columns = await dbService.GetSchemaAsync(ConnectionString, SqlQuery);

            if (GenerateType?.Contains("Properties") == true)
            {
                Result = _codeGenerator.GeneratePropertiesOnly(columns);
            }
            else
            {
                Result = _codeGenerator.GenerateClass("GeneratedDto", columns);
            }
        }
        catch (Exception ex)
        {
            Result = $"// Hata oluştu:\n{ex.Message}";
        }
        finally
        {
            IsGenerating = false;
        }
    }

    private IDatabaseService GetDatabaseService(string dbType)
    {
        return dbType switch
        {
            "SqlServer" => new SqlServerProvider(),
            _ => throw new NotImplementedException($"'{dbType}' henüz desteklenmiyor.")
        };
    }

    [RelayCommand]
    private void SaveSettings()
    {
        Result = "// Ayarlar kaydedildi (Henüz implante edilmedi)";
    }
}