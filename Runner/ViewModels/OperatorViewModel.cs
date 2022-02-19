using AbstractionProvider.Configuration;

namespace Runner.ViewModels;

public class OperatorViewModel : ViewModelBase
{
    public IConfiguration Configuration { get; }

    public OperatorViewModel(IConfiguration configuration)
    {
        Configuration = configuration;
    }
}