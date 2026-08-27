namespace HERMESLANG.Parser;

public class AssignmentStatement : Statement
{
    public string Name { get; }
    public Expression Value { get; }

    public AssignmentStatement(string name, Expression value)
    {
        Name = name;
        Value = value;
    }
}