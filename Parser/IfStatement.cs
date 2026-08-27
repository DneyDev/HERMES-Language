namespace HERMESLANG.Parser;

public class IfStatement : Statement
{
    public Expression Condition { get; }
    public List<Statement> Body { get; }

    public IfStatement(
        Expression condition,
        List<Statement> body)
    {
        Condition = condition;
        Body = body;
    }
}