namespace HERMESLANG.Runtime;

public class HermesException : Exception
{
    public int? Line {get;}

    public HermesException(
        string message,
        int? line = null)
        : base(message)
    {
        Line = line;
    }
}