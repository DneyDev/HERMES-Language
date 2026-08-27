namespace HERMESLANG.Parser;

public class VariableExpression : Expression
{
    public string Name{get;}

    public VariableExpression(string name)
    {
            Name = name;
    }
}