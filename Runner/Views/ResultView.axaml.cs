using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Runner.Views;

public class ResultView : Window
{
    public ResultView(string data)
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.DataContext = data;
    }

    public ResultView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}