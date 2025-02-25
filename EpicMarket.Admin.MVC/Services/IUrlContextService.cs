public interface IUrlContextService
{
    string CurrentPageUrl { get; }
}

public class UrlContextService : IUrlContextService
{
    private string _currentPageUrl;

    public string CurrentPageUrl 
    { 
        get => _currentPageUrl;
        internal set => _currentPageUrl = value;
    }
} 