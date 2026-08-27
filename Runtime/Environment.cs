namespace HERMESLANG.Runtime;

public class Environment
{
    private readonly Dictionary<string, object?> _values = new();

    public void Define(string name, object? value)
    {
        _values[name] = value;
    }

    public object? Get(string name)
    {
        if (_values.TryGetValue(name, out object? value))
        {
            return value;
        }

        throw new Exception(
            $"Variável '{name}' não foi definida."
        );
    }

    public bool Exists(string name)
    {
        return _values.ContainsKey(name);
    }
    public Dictionary<string, object?> GetAll()
    {
        return new Dictionary<string, object?>(_values);
    }
}