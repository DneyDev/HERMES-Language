namespace HERMESLANG.Runtime.Functions;

public class MostrarFunction : IHermesFunction
{
    public string Name => "mostrar";

    public int Arity => 1;

    public object? Execute(List<object?> arguments)
    {
        Console.WriteLine(arguments[0]);

        return null;
    }
}