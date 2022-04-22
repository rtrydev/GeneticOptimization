namespace Runner.ViewModels;

public class HelpViewModel : ViewModelBase
{
    public string Version { get; }

    public HelpViewModel()
    {
        Version = $"Genetic Optimizer v{typeof(MainWindowViewModel).Assembly.GetName().Version?.ToString()} by Rafał Kulka 2022";
    }
}