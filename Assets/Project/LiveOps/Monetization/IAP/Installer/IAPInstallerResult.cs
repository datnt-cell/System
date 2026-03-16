using IAPModule.Infrastructure;

public class IAPInstallerResult
{
    public IAPService Service { get; }
    public IAPEvents Events { get; }

    public IAPInstallerResult(
        IAPService service,
        IAPEvents events)
    {
        Service = service;
        Events = events;
    }
}