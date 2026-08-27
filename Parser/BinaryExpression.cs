namespace HERMESLANG.Parser;

public class BinaryExpression : Expression
{
    public Expression Left { get; }
    public HERMESLANG.Lexer.Token Operator { get; }
    public Expression Right { get; }
    public BinaryExpression(
        Expression left,
        HERMESLANG.Lexer.Token @operator,
        Expression right)
    {
        Left = left;
        Operator = @operator;
        Right = right;
    }
}