namespace SimplePatterns
{
    public class AndroidApplication : IApplication
    {
        public IStyle Style { get; set; }

        public IFrontend Frontend { get; set; }

        public IBackend Backend { get; set; }
    }
}