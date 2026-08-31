namespace HERMESLANG.Runtime.Functions;

public class AbrirPortaFunction : IHermesFunction
{
    public string Name => "abrir_porta";

    public int Arity => 0;

    public object? Execute(List<object?> arguments)
    {
        Console.WriteLine("Porta aberta!");

        return null;
    }
}