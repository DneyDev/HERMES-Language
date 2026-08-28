using HERMESLANG.Lexer;

namespace HERMESLANG.Parser;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _current;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public List<Statement> Parse()
    {
        List<Statement> statements = new();

        while (!IsAtEnd())
        {
            if (Match(TokenType.NewLine))
                continue;

            statements.Add(ParseStatement());
        }

        return statements;
    }

    private Statement ParseStatement()
    {
        if (Match(TokenType.Se))
        {
            return ParseIfStatement();
        }

        if (Check(TokenType.Identifier) &&
            CheckNext(TokenType.Assign))
        {
            return ParseAssignment();
        }

        return new ExpressionStatement(ParseExpression());
    }

    private Statement ParseAssignment()
    {
        Token name = Consume(
            TokenType.Identifier,
            "Esperado nome da variável."
        );

        Consume(
            TokenType.Assign,
            "Esperado '=' após o nome da variável."
        );

        Expression value = ParseExpression();

        ConsumeOptionalNewLine();

        return new AssignmentStatement(
            name.Lexeme,
            value
        );
    }

    private Statement ParseIfStatement()
    {
        Expression condition = ParseExpression();

        Consume(
            TokenType.Colon,
            "Esperado ':' após a condição."
        );

        Consume(
            TokenType.NewLine,
            "Esperado nova linha após ':'."
        );

        Consume(
            TokenType.Indent,
            "Esperado bloco indentado."
        );

        List<Statement> body = new();

        while (!Check(TokenType.Dedent) &&
               !IsAtEnd())
        {
            if (Match(TokenType.NewLine))
                continue;

            body.Add(ParseStatement());
        }

        Consume(
            TokenType.Dedent,
            "Esperado fim do bloco."
        );

        List<Statement> elseBody = new();

        if (Match(TokenType.Senao))
        {
            Consume(
                TokenType.Colon,
                "Esperado ':' após 'senão'."
            );
            Consume(
                TokenType.NewLine,
                "Esperado nova linha após ':'."
            );
            Consume(
                TokenType.Indent,
                "Esperado bloco indentado após 'senão'."
            );
            while(!Check(TokenType.Dedent) && !IsAtEnd())
            {
                if(Match(TokenType.NewLine)) continue;
                elseBody.Add(ParseStatement());
            }
            Consume(
                TokenType.Dedent,
                "Esperado fim do bloco 'senão'."
            );
        }

        return new IfStatement(
            condition,
            body,
            elseBody
        );
    }

    private Expression ParseExpression()
    {
        return ParseOr();
    }

    private Expression ParseOr()
    {
        Expression expression = ParseAnd();

        while (Match(TokenType.Ou))
        {
            Token operatorToken = Previous();

            Expression right = ParseAnd();

            expression = new BinaryExpression(
                expression,
                operatorToken,
                right
            );
        }
        return expression;
    }

    private Expression ParseAnd()
    {
        Expression expression = ParseNot();

        while (Match(TokenType.E))
        {
            Token operatorToken = Previous();

            Expression right = ParseNot();

            expression = new BinaryExpression(
                expression,
                operatorToken,
                right
            );
        }
        return expression;
    }

    private Expression ParseNot()
    {
        if (Match(TokenType.Nao))
        {
            Token operatorToken = Previous();

            Expression right = ParseNot();

            return new UnaryExpression(
                operatorToken,
                right
            );
        }
        return ParseComparison();
    }

    private Expression ParseComparison()
    {
        Expression expression = ParseTerm();

        while (Match(
            TokenType.EqualEqual,
            TokenType.NotEqual,
            TokenType.Greater,
            TokenType.GreaterEqual,
            TokenType.Less,
            TokenType.LessEqual))
        {
            Token operatorToken = Previous();

            Expression right = ParseTerm();

            expression = new BinaryExpression(
                expression,
                operatorToken,
                right
            );
        }

        return expression;
    }
    private Expression ParseTerm()
    {
        Expression expression = ParseFactor();

        while (Match(
            TokenType.Plus,
            TokenType.Minus))
        {
            Token operatorToken = Previous();

            Expression right = ParseFactor();

            expression = new BinaryExpression(
                expression,
                operatorToken,
                right
            );
        }

        return expression;
    }

    private Expression ParsePrimary()
    {
        if (Match(TokenType.Number))
        {
            return new LiteralExpression(
                Previous().Literal
            );
        }

        if (Match(TokenType.String))
        {
            return new LiteralExpression(
                Previous().Literal
            );
        }

        if (Match(TokenType.Boolean))
        {
            return new LiteralExpression(
                Previous().Literal
            );
        }

        if (Match(TokenType.Identifier))
        {
            Token name = Previous();

            if (Match(TokenType.LeftParenthesis))
            {
                return ParseCall(name);
            }

            return new VariableExpression(
                name.Lexeme
            );
        }

        throw Error(
            Peek(),
            "Expressão inválida."
        );
    }
    
    private Expression ParseCall(Token name)
    {
        List<Expression> arguments = new();

        if (!Check(TokenType.RightParenthesis))
        {
            do
            {
                arguments.Add(ParseExpression());
            }
            while (Match(TokenType.Comma));
        }

        Consume(
            TokenType.RightParenthesis,
            "Esperado ')' após os argumentos."
        );

        return new CallExpression(
            name.Lexeme,
            arguments
        );
    }

    private Expression ParseFactor()
    {
        Expression expression = ParsePrimary();

        while(Match(
            TokenType.Multiply,
            TokenType.Divide
        ))
        {
            Token operatorToken = Previous();

            Expression right = ParsePrimary();

            expression = new BinaryExpression(
                expression,
                operatorToken,
                right
            );
        }
        return expression;
    }

    private bool Match(params TokenType[] types)
    {
        foreach (TokenType type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }

        return false;
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd())
            return type == TokenType.EndOfFile;

        return Peek().Type == type;
    }

    private bool CheckNext(TokenType type)
    {
        if (_current + 1 >= _tokens.Count)
            return false;

        return _tokens[_current + 1].Type == type;
    }

    private Token Advance()
    {
        if (!IsAtEnd())
            _current++;

        return Previous();
    }

    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EndOfFile;
    }

    private Token Peek()
    {
        return _tokens[_current];
    }

    private Token Previous()
    {
        return _tokens[_current - 1];
    }

    private Token Consume(
        TokenType type,
        string message)
    {
        if (Check(type))
            return Advance();

        throw Error(Peek(), message);
    }

    private void ConsumeOptionalNewLine()
    {
        Match(TokenType.NewLine);
    }

    private ParserException Error(
        Token token,
        string message)
    {
        return new ParserException(
            $"Linha {token.Line}: {message}"
        );
    }
}