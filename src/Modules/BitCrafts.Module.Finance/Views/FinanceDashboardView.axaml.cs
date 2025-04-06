using BitCrafts.Application.Avalonia.Controls.Views;
using BitCrafts.Module.Finance.Abstraction.Views;

namespace BitCrafts.Module.Finance.Views;

public partial class FinanceDashboardView : BaseView, IFinanceDashboardView
{
    public FinanceDashboardView()
    {
        InitializeComponent();
    }


    public override void ShowError(string message)
    {
    }

    protected override void OnAppeared()
    {
    }

    protected override void OnDisappeared()
    {
    }
}