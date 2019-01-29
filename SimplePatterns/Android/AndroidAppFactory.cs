namespace SimplePatterns
{
    class AndroidAppFactory: IApplicationFactory
    {
        public IStyle CreateStyle()
        {
            return new AndroidStyle();
        }

        public IFrontend CreateFrontend()
        {
            return new AndroidFrontend();
        }

        public IBackend CreateBackend()
        {
            return new AndroidBackend();
        }

        public IApplication CreateApplication()
        {
            return new AndroidApplication {
                Backend = CreateBackend(),
                Frontend = CreateFrontend(),
                Style = CreateStyle()
            };
        }
    }
}