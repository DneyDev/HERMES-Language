namespace HERMESLANG.Parser;

public class ExpressionStatement : Statement
{
    public Expression Expression { get; }

    public ExpressionStatement(Expression expression)
    {
        Expression = expression;
    }
}