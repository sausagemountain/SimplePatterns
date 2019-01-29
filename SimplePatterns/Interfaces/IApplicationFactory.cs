namespace SimplePatterns
{
    public interface IApplicationFactory
    {
        IStyle CreateStyle();

        IFrontend CreateFrontend();

        IBackend CreateBackend();

        IApplication CreateApplication();
    }
}