namespace SimplePatterns
{
    public interface IApplication
    {
        IStyle Style { get; set; }

        IFrontend Frontend { get; set; }

        IBackend Backend { get; set; }
    }
}