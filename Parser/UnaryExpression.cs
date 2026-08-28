using HERMESLANG.Lexer;

namespace HERMESLANG.Parser;

public class UnaryExpression : Expression
{
    public Token Operator { get; }
    public Expression Right { get; }

    public UnaryExpression(
        Token @operator,
        Expression right
    )
    {
        Operator = @operator;
        Right = right;
    }
}