using BitCrafts.Application.Avalonia.Controls.Views;
using BitCrafts.Module.Finance.Abstraction.Views;

namespace BitCrafts.Module.Finance.Views;

public partial class FinanceDashboardView : BaseControl, IFinanceDashboardView
{
    public FinanceDashboardView()
    {
        InitializeComponent();
    }


    protected override void OnAppeared()
    {
    }

    protected override void OnDisappeared()
    {
    }
}