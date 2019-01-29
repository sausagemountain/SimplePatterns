namespace SimplePatterns
{
    public class IosAppFactory : IApplicationFactory
    {
        public IStyle CreateStyle()
        {
            return new IosStyle();
        }

        public IFrontend CreateFrontend()
        {
            return new IosFrontend();
        }

        public IBackend CreateBackend()
        {
            return new IosBackend();
        }

        public IApplication CreateApplication()
        {
            return new IosApplication {
                Backend = CreateBackend(),
                Frontend = CreateFrontend(),
                Style = CreateStyle()
            };
        }
    }
}