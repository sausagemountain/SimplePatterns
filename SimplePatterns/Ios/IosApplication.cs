namespace SimplePatterns
{
    public class IosApplication : IApplication
    {
        public IStyle Style { get; set; }

        public IFrontend Frontend { get; set; }

        public IBackend Backend { get; set; }
    }
}