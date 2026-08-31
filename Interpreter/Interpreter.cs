using HERMESLANG.Parser;
using HERMESLANG.Runtime;

namespace HERMESLANG.Interpreter;

public class Interpreter
{
    private readonly HERMESLANG.Runtime.Environment _environment;

    public Interpreter()
    {
        _environment = new HERMESLANG.Runtime.Environment();
        _functionRegistry = new FunctionRegistry();
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

            case WhileStatement whileStatement:
                ExecuteWhile(whileStatement);
                break;
            default:
                throw new Exception(
                    $"Statement não suportado: {statement.GetType().Name}"
                );
        }
    }

    private void ExecuteAssignment(AssignmentStatement statement)
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

    private void ExecuteWhile(WhileStatement statement)
    {
        int iterations = 0;
        const int maxIterations = 10000;

        while (IsTruthy(Evaluate(statement.Condition)))
        {
            iterations++;

            if (iterations > maxIterations)
            {
                throw new Exception(
                    "O loop 'enquanto' excedeu o limite de execuções."
                );
            }

            foreach (Statement bodyStatement in statement.Body)
            {
                Execute(bodyStatement);
            }
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

    private object? EvaluateBinary(BinaryExpression expression)
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
                => Add(left, right),
            
            HERMESLANG.Lexer.TokenType.Minus
                => Subtract(left, right),

            HERMESLANG.Lexer.TokenType.Multiply
                => Multiply(left, right),

            HERMESLANG.Lexer.TokenType.Divide
                => Divide(left, right),

            HERMESLANG.Lexer.TokenType.Greater
                => CompareNumbers(left, right, ">"),

            HERMESLANG.Lexer.TokenType.Less
                => CompareNumbers(left, right, "<"),

            HERMESLANG.Lexer.TokenType.GreaterEqual
                => CompareNumbers(left, right, ">="),

            HERMESLANG.Lexer.TokenType.LessEqual
                => CompareNumbers(left, right, "<="),

            HERMESLANG.Lexer.TokenType.EqualEqual
                => Equals(left, right),

            HERMESLANG.Lexer.TokenType.NotEqual
                => !Equals(left, right),

            _ => throw new Exception(
                $"Operador não suportado: {expression.Operator.Type}"
            )
        };
    }
    //métodos matemáticos definidos abaixo
    private object Add(object? left, object? right) //somar
    {
        if (left is double leftNumber && right is double rightNumber)
        {
            return leftNumber + rightNumber;
        }
        if(left is string leftString && right is string rightString)
        {
            return leftString + rightString;
        }

        throw new Exception(
            "O operador '+' só pode ser usado entre números ou textos."
        );
    }

    private object Subtract(object? left, object? right)//subtrair
    {
        if(left is double leftNumber && right is double rightNumber)
        {
            return leftNumber - rightNumber;
        }
        throw new Exception(
            "O operador '-' só pode ser usado entre números."
        );
    }

    private object Multiply(object? left, object? right)//multiplicar
    {
        if(left is double leftNumber && right is double rightNumber)
        {
            return leftNumber * rightNumber;
        }
        throw new Exception(
            "O operador '*' só pode ser usado entre números."
        );
    }

    private object Divide(object? left, object? right)//dividir
    {
        if(left is double leftNumber && right is double rightNumber)
        {
            if(rightNumber == 0)
            {
                throw new Exception(
                    "Não é possível dividir por zero."
                );
            }
            return leftNumber / rightNumber;
        }
        throw new Exception(
            "O operador '/' só pode ser usado entre números."
        );
    }

    private bool CompareNumbers(object? left, object? right, string operatorSymbol)
    {
        if(left is not double leftNumber || right is not double rightNumber)
        {
            throw new Exception(
                $"O operador '{operatorSymbol}' só pode ser usado entre números."
            );
        }
        return operatorSymbol switch
        {
            ">" => leftNumber > rightNumber,
            "<" => leftNumber < rightNumber,
            ">=" => leftNumber >= rightNumber,
            "<=" => leftNumber <= rightNumber,

            _ => throw new Exception(
                $"Operador de comparação inválido: {operatorSymbol}"
            )
        };
    }
    private readonly FunctionRegistry _functionRegistry;

    public void RegisterFunction(IHermesFunction function)
    {
        _functionRegistry.Register(function);
    }
    private object? EvaluateCall(CallExpression expression)
    {
        List<object?> arguments = new();

        foreach(Expression argument in expression.Arguments)
        {
            arguments.Add(Evaluate(argument));
        }

        return _functionRegistry.Call(expression.Name, arguments);
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