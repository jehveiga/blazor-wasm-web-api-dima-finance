using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Globalization;

namespace Dima.Web.Components.Reports;

public partial class IncomesAndExpensesChartComponent : ComponentBase
{
    #region Properties

    public ChartOptions Options { get; set; } = new();
    public List<ChartSeries>? Series { get; set; }
    public List<string> Labels { get; set; } = [];

    #endregion Properties

    #region Services

    [Inject]
    public IReportHandler Handler { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion Services

    #region Override

    protected override async Task OnInitializedAsync()
    {
        try
        {
            GetIncomesAndExpensesRequest request = new();
            Core.Responses.Response<List<IncomesAndExpenses>?> result = await Handler.GetIncomesAndExpensesReportAsync(request);
            if (!result.IsSuccess || result.Data is null)
            {
                Snackbar.Add("Não foi possível obter os dados do relatório", Severity.Error);
                return;
            }

            List<double> incomes = [];
            List<double> expenses = [];

            foreach (IncomesAndExpenses item in result.Data)
            {
                incomes.Add((double)item.Incomes);
                expenses.Add(-(double)item.Expenses);
                Labels.Add(GetMonthName(item.Month));
            }

            Options.YAxisTicks = 1000;
            Options.LineStrokeWidth = 5;
            Options.ChartPalette = ["#76FF01", Colors.Red.Default];

            Series =
                    [
                        new ChartSeries { Name = "Receitas", Data = incomes.ToArray() },
                        new ChartSeries { Name = "Saídas", Data = expenses.ToArray() }
                    ];
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
            return;
        }

        StateHasChanged();
    }

    #endregion Override

    private static string GetMonthName(int month)
        => new DateTime(DateTime.Now.Year, month, 1)
            .ToString("MMMM", CultureInfo.CurrentCulture);
}