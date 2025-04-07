using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Application.Avalonia.Controls.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BitCrafts.Application.Avalonia.Views;

public partial class EnvironmentConfigurationView : EditableView<EnvironmentConfigurationViewModel>,
    IEnvironmentConfigurationView
{
    public EnvironmentConfigurationView()
    {
        InitializeComponent();
    }

    protected override Control LoadingIndicator => LoadingControl;

    protected override TextBlock ErrorTextBlock => LoadingMessageTextBox;

    protected override void OnDataDisplayed(EnvironmentConfigurationViewModel model)
    {
    }
}