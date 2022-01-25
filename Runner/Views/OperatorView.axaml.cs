using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Runner.Views;

public class OperatorView : UserControl
{
    public OperatorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}