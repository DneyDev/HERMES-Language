namespace HERMESLANG.Runtime.Errors;

public class InvalidTypeException : HermesException
{
    public InvalidTypeException(
        string message,
        int? line = null
    ) : base(message, line)
    {
        
    }
}