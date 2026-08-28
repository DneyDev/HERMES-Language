using HERMESLANG.Parser;
using HERMESLANG.Runtime;

namespace HERMESLANG.Interpreter;

public class Interpreter
{
    private readonly HERMESLANG.Runtime.Environment _environment;

    public Interpreter()
    {
        _environment = new HERMESLANG.Runtime.Environment();
    }

    public void Interpret(List<Statement> statements)
    {
        foreach (Statement statement in statements)
        {
            Execute(statement);
        }
    }

    private void Execute(Statement statement)
    {
        switch (statement)
        {
            case AssignmentStatement assignment:
                ExecuteAssignment(assignment);
                break;

            case ExpressionStatement expressionStatement:
                Evaluate(expressionStatement.Expression);
                break;

            case IfStatement ifStatement:
                ExecuteIf(ifStatement);
                break;

            default:
                throw new Exception(
                    $"Statement não suportado: {statement.GetType().Name}"
                );
        }
    }

    private void ExecuteAssignment(
        AssignmentStatement statement)
    {
        object? value = Evaluate(statement.Value);

        _environment.Define(
            statement.Name,
            value
        );
    }

    private void ExecuteIf(IfStatement statement)
    {
        object? condition = Evaluate(statement.Condition);

        if (IsTruthy(condition))
        {
            foreach (Statement bodyStatement in statement.Body)
            {
                Execute(bodyStatement);
            }

            return;
        }

        foreach(Statement elseStatement in statement.ElseBody)
        {
            Execute(elseStatement);
        }
    }

    private object? Evaluate(Expression expression)
    {
        switch (expression)
        {
            case LiteralExpression literal:
                return literal.Value;

            case VariableExpression variable:
                return _environment.Get(variable.Name);

            case BinaryExpression binary:
                return EvaluateBinary(binary);

            case UnaryExpression unary:
                return EvaluateUnary(unary);    

            case CallExpression call:
                return EvaluateCall(call);

            default:
                throw new Exception(
                    $"Expression não suportada: {expression.GetType().Name}"
                );
        }
    }

    private object? EvaluateBinary(
        BinaryExpression expression)
    {
        object? left = Evaluate(expression.Left);
        object? right = Evaluate(expression.Right);

        return expression.Operator.Type switch //definição dos tipos de Token
        {
            HERMESLANG.Lexer.TokenType.E
                => IsTruthy(left) && IsTruthy(right),

            HERMESLANG.Lexer.TokenType.Ou
                => IsTruthy(left) || IsTruthy(right),

            HERMESLANG.Lexer.TokenType.Plus
                => Convert.ToDouble(left) + Convert.ToDouble(right),
            
            HERMESLANG.Lexer.TokenType.Minus
                => Convert.ToDouble(left) - Convert.ToDouble(right),

            HERMESLANG.Lexer.TokenType.Multiply
                => Convert.ToDouble(left) * Convert.ToDouble(right),

            HERMESLANG.Lexer.TokenType.Divide
                => Convert.ToDouble(left) / Convert.ToDouble(right),

            HERMESLANG.Lexer.TokenType.Greater
                => Convert.ToDouble(left) > Convert.ToDouble(right),

            HERMESLANG.Lexer.TokenType.Less
                => Convert.ToDouble(left) < Convert.ToDouble(right),

            HERMESLANG.Lexer.TokenType.GreaterEqual
                => Convert.ToDouble(left) >= Convert.ToDouble(right),

            HERMESLANG.Lexer.TokenType.LessEqual
                => Convert.ToDouble(left) <= Convert.ToDouble(right),

            HERMESLANG.Lexer.TokenType.EqualEqual
                => Equals(left, right),

            HERMESLANG.Lexer.TokenType.NotEqual
                => !Equals(left, right),

            _ => throw new Exception(
                $"Operador não suportado: {expression.Operator.Type}"
            )
        };
    }

    private object? EvaluateCall(
        CallExpression expression)
    {
        throw new Exception(
            $"Função '{expression.Name}' ainda não está registrada."
        );
    }

    private object? EvaluateUnary(UnaryExpression expression)
    {
        object? right = Evaluate(expression.Right);

        return expression.Operator.Type switch
        {
            HERMESLANG.Lexer.TokenType.Nao
                => !IsTruthy(right),

            _ => throw new Exception(
                $"Operador unário não suportado: {expression.Operator.Type}"
             )
        };
    }

    private bool IsTruthy(object? value)
    {
        if (value is bool boolean)
        {
            return boolean;
        }

        if (value is null)
        {
            return false;
        }

        return true;
    }
    public Dictionary<string, object?> GetVariables()
    {
        return _environment.GetAll();
    }
}