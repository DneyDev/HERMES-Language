namespace HERMESLANG.Runtime;

public interface IHermesFunction
{
    string Name {get;}

    int Arity {get;}
    object? Execute(List<object?> arguments);
}