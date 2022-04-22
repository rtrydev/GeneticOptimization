using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Runner.ViewModels;

namespace Runner.Views;

public class HelpView : Window
{
    public HelpView()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        this.DataContext = new HelpViewModel();
    }
}