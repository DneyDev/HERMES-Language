using System.Security.Cryptography.X509Certificates;

namespace HERMESLANG.Runtime.Errors;

public class DivisionByZeroException : HermesException
{
    public DivisionByZeroException(
        int? line = null
    ) : base(
        "Não é possível dividir por zero.",
        line
    )
    {
    }
}