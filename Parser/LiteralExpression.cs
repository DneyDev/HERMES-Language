namespace HERMESLANG.Parser;

public class LiteralExpression : Expression
{
    public object? Value{get;}

    public LiteralExpression(object? value)
    {
        Value = value;
    }
}