using HERMESLANG.Parser;

namespace HERMESLANG.Runtime;

public class FunctionRegistry
{
    private readonly Dictionary<string, Func<List<object?>, object?>> _functions = new();

    public void Register(string name, Func<List<object?>, object?> function)
    {
        _functions[name]= function;
    }
    public bool Exists(string name)
    {
        return _functions.ContainsKey(name);
    }
    public object? Call(string name, List<object?> arguments)
    {
        if(!_functions.TryGetValue(name, out var function))
        {
            throw new Exception(
                $"Função '{name}' não está resgistrada."
            );
        }

        return function(arguments);
    }
}