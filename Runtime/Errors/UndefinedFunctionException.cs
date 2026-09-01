namespace HERMESLANG.Runtime.Errors;

public class UndefinedFunctionException : HermesException
{
    public UndefinedFunctionException(
        string name,
        int? line = null)
        : base(
            $"Função '{name}' não está registrada.",
            line
        )
    {
    }
}