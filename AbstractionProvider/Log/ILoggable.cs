namespace AbstractionProvider.Log;

public interface ILoggable
{
    public void AttachLogger(ILogger logger);
    public ILogger GetLogger();
}