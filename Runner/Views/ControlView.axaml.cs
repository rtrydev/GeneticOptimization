using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Runner.ViewModels;

namespace Runner.Views;

public class ControlView : UserControl
{
    
    public ControlView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}