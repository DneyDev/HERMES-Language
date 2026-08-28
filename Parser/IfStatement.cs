namespace HERMESLANG.Parser;
//Definição de if/else do Parser
public class IfStatement : Statement
{
    public Expression Condition { get; }
    public List<Statement> Body { get; }

    public List<Statement> ElseBody { get; }

    public IfStatement(
        Expression condition,
        List<Statement> body,
        List<Statement> elseBody)
    {
        Condition = condition;
        Body = body;
        ElseBody = elseBody;
    }
}