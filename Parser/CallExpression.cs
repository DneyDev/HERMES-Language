using HERMESLANG.Lexer;

namespace HERMESLANG.Parser;

public class CallExpression : Expression
{
    public string Name { get; }
    public List<Expression> Arguments { get; }

    public CallExpression(string name, List<Expression> arguments)
    {
        Name = name;
        Arguments = arguments;
    }
}