using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Runner.ViewModels;

namespace Runner.Views;

public class HistoryView : UserControl
{
    public HistoryView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}