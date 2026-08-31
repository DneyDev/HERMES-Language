namespace HERMESLANG.Runtime;

public class FunctionRegistry
{
    private readonly Dictionary<string, IHermesFunction> _functions = new();

    public void Register(IHermesFunction function)
    {
        _functions[function.Name] = function;
    }

    public bool Exists(string name)
    {
        return _functions.ContainsKey(name);
    }

    public object? Call(
        string name,
        List<object?> arguments)
    {
        if (!_functions.TryGetValue(name, out IHermesFunction? function))
        {
            throw new Exception(
                $"Função '{name}' não está registrada."
            );
        }

        if (arguments.Count != function.Arity)
        {
            throw new Exception(
                $"A função '{name}' espera {function.Arity} argumento(s), " +
                $"mas recebeu {arguments.Count}."
            );
        }

        return function.Execute(arguments);
    }
}