namespace NSLangAnalyzer;

public class NSLException : Exception
{
    public NSLException()
    {
    }

    public NSLException(string message) : base(message)
    {
    }

    public NSLException(string message, Exception exception) : base(message, exception)
    {
    }
}