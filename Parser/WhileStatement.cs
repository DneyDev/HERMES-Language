namespace HERMESLANG.Parser;

public class WhileStatement : Statement
{
    public Expression Condition { get; }
    public List<Statement> Body { get; }

    public WhileStatement(
        Expression condition,
        List<Statement> body)
    {
        Condition = condition;
        Body = body;
    }
}