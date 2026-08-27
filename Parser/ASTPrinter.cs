using HERMESLANG.Lexer;

namespace HERMESLANG.Parser;

public class ASTPrinter
{
    private readonly List<string> _lines = new();

    public void Print(List<Statement> statements)
    {
        _lines.Clear();

        foreach (Statement statement in statements)
        {
            PrintStatement(statement, 0);
        }

        foreach (string line in _lines)
        {
            Console.WriteLine(line);
        }
    }

    private void PrintStatement(Statement statement, int level)
    {
        if (statement is AssignmentStatement assignment)
        {
            AddLine(level, "AssignmentStatement");
            AddLine(level + 1, $"Nome: {assignment.Name}");

            AddLine(level + 1, "Valor:");
            PrintExpression(assignment.Value, level + 2);

            return;
        }

        if (statement is IfStatement ifStatement)
        {
            AddLine(level, "IfStatement");

            AddLine(level + 1, "Condição:");
            PrintExpression(ifStatement.Condition, level + 2);

            AddLine(level + 1, "Corpo:");

            foreach (Statement bodyStatement in ifStatement.Body)
            {
                PrintStatement(bodyStatement, level + 2);
            }

            return;
        }

        if (statement is ExpressionStatement expressionStatement)
        {
            AddLine(level, "ExpressionStatement");
            PrintExpression(expressionStatement.Expression, level + 1);

            return;
        }

        AddLine(level, $"Statement desconhecido: {statement.GetType().Name}");
    }

    private void PrintExpression(Expression expression, int level)
    {
        if (expression is LiteralExpression literal)
        {
            AddLine(level, $"Literal: {literal.Value}");
            return;
        }

        if (expression is VariableExpression variable)
        {
            AddLine(level, $"Variable: {variable.Name}");
            return;
        }

        if (expression is BinaryExpression binary)
        {
            AddLine(level, "BinaryExpression");

            AddLine(level + 1, "Esquerda:");
            PrintExpression(binary.Left, level + 2);

            AddLine(
                level + 1,
                $"Operador: {binary.Operator.Type}"
            );

            AddLine(level + 1, "Direita:");
            PrintExpression(binary.Right, level + 2);

            return;
        }

        if (expression is CallExpression call)
        {
            AddLine(level, $"CallExpression: {call.Name}");

            if (call.Arguments.Count > 0)
            {
                AddLine(level + 1, "Argumentos:");

                foreach (Expression argument in call.Arguments)
                {
                    PrintExpression(argument, level + 2);
                }
            }
            else
            {
                AddLine(level + 1, "Argumentos: nenhum");
            }

            return;
        }

        AddLine(
            level,
            $"Expression desconhecida: {expression.GetType().Name}"
        );
    }

    private void AddLine(int level, string text)
    {
        _lines.Add($"{new string(' ', level * 4)}{text}");
    }
}