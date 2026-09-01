namespace HERMESLANG.Runtime.Errors;

public class FunctionArityException : HermesException
{
    public FunctionArityException(
        string name,
        int expected,
        int received,
        int? line = null
    ) : base(
        $"A função '{name}' espera {expected} argumento(s), mas recebeu {received}.",
        line
    )
    {
    }
}