using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Runner.Models;
using Runner.ViewModels;

namespace Runner.Views;

public class ResultView : Window
{
    public ResultView(ResultFileModel data)
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.DataContext = new ResultViewModel(data);
        var app = Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime;
        this.Owner = app.MainWindow;
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