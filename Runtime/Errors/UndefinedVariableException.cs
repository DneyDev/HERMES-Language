namespace HERMESLANG.Runtime.Errors;

public class UndefinedVariableException : HermesException
{
    public UndefinedVariableException(
        string name,
        int? line = null)
        : base(
            $"Variável '{name}' não foi definida.",
            line
        )
    {
    }
}