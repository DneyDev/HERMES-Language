using System.Globalization;

namespace HERMESLANG.Lexer;

public class Lexer
{
    private readonly string _source;
    private readonly List<Token> _tokens = new();

    private readonly List<int> _indentLevels = new() { 0 };
    private bool _atLineStart = true;

    private int _start;
    private int _current;
    private int _line = 1;

    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        { "se", TokenType.Se },
        { "senão", TokenType.Senao },
        { "e", TokenType.E },
        { "ou", TokenType.Ou },
        { "não", TokenType.Nao },
        { "Verdadeiro", TokenType.Boolean },
        { "Falso", TokenType.Boolean }
    };

    public Lexer(string source)
    {
        _source = source;
    }

    private void HandleIndentation()
    {
        int spaces = 0;

        while (!IsAtEnd() && Peek() == ' ')
        {
            Advance();
            spaces++;
        }

        // Linha vazia
        if (IsAtEnd() || Peek() == '\n')
        {
            if (!IsAtEnd())
            {
                Advance();
                _line++;
            }

            _atLineStart = true;
            return;
        }

        int currentIndent = _indentLevels[^1];

        if (spaces > currentIndent)
        {
            _indentLevels.Add(spaces);

            _tokens.Add(new Token(
                TokenType.Indent,
                "",
                null,
                _line
            ));
        }
        else if (spaces < currentIndent)
        {
            while (_indentLevels.Count > 1 && spaces < _indentLevels[^1])
            {
                _indentLevels.RemoveAt(_indentLevels.Count - 1);

                _tokens.Add(new Token(
                    TokenType.Dedent,
                    "",
                    null,
                    _line
                ));
            }
        }

        _atLineStart = false;
    }

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _start = _current;

            if (_atLineStart)
            {
                HandleIndentation();

                if (_current >= _source.Length)
                    break;
            }

            ScanToken();
        }

        // Fecha possíveis blocos ainda abertos.
        while (_indentLevels.Count > 1)
        {
            _indentLevels.RemoveAt(_indentLevels.Count - 1);

            _tokens.Add(new Token(
                TokenType.Dedent,
                "",
                null,
                _line
            ));
        }

        _tokens.Add(new Token(
            TokenType.EndOfFile,
            "",
            null,
            _line
        ));

        return _tokens;
    }

    private void ScanToken()
    {
        char c = Advance();

        switch (c)
        {
            case '(':
                AddToken(TokenType.LeftParenthesis);
                break;

            case ')':
                AddToken(TokenType.RightParenthesis);
                break;

            case ':':
                AddToken(TokenType.Colon);
                break;

            case '+':
                AddToken(TokenType.Plus);
                break;

            case '-':
                AddToken(TokenType.Minus);
                break;

            case '*':
                AddToken(TokenType.Multiply);
                break;

            case '/':
                AddToken(TokenType.Divide);
                break;

            case '=':
                AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Assign);
                break;

            case '!':
                if (Match('='))
                    AddToken(TokenType.NotEqual);
                break;

            case '>':
                AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater);
                break;

            case '<':
                AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less);
                break;

            case '"':
                String();
                break;

            case '#':
                Comment();
                break;

            case ' ':
            case '\r':
            case '\t':
                break;

            case '\n':
                _line++;
                _atLineStart = true;
                AddToken(TokenType.NewLine);
                break;

            default:
                if (char.IsDigit(c))
                {
                    Number();
                }
                else if (char.IsLetter(c) || c == '_')
                {
                    Identifier();
                }

                break;
        }
    }

    private void Number()
    {
        while (!IsAtEnd() && char.IsDigit(Peek()))
        {
            Advance();
        }

        // Decimal
        if (!IsAtEnd() && Peek() == '.' &&
            char.IsDigit(PeekNext()))
        {
            Advance();

            while (!IsAtEnd() && char.IsDigit(Peek()))
            {
                Advance();
            }
        }

        string text = _source[_start.._current];

        double value = double.Parse(
            text,
            CultureInfo.InvariantCulture
        );

        AddToken(TokenType.Number, value);
    }

    private void String()
    {
        while (!IsAtEnd() && Peek() != '"')
        {
            if (Peek() == '\n')
                _line++;

            Advance();
        }

        if (IsAtEnd())
        {
            return;
        }

        Advance();

        string value = _source[(_start + 1)..(_current - 1)];

        AddToken(TokenType.String, value);
    }

    private void Identifier()
    {
        while (!IsAtEnd() &&
               (char.IsLetterOrDigit(Peek()) || Peek() == '_'))
        {
            Advance();
        }

        string text = _source[_start.._current];

        if (Keywords.TryGetValue(text, out TokenType type))
        {
            if (type == TokenType.Boolean)
            {
                bool value = text == "Verdadeiro";
                AddToken(type, value);
            }
            else
            {
                AddToken(type);
            }

            return;
        }

        AddToken(TokenType.Identifier);
    }

    private void Comment()
    {
        while (!IsAtEnd() && Peek() != '\n')
        {
            Advance();
        }
    }

    private bool Match(char expected)
    {
        if (IsAtEnd())
            return false;

        if (_source[_current] != expected)
            return false;

        _current++;
        return true;
    }

    private char Advance()
    {
        return _source[_current++];
    }

    private char Peek()
    {
        if (IsAtEnd())
            return '\0';

        return _source[_current];
    }

    private char PeekNext()
    {
        if (_current + 1 >= _source.Length)
            return '\0';

        return _source[_current + 1];
    }

    private bool IsAtEnd()
    {
        return _current >= _source.Length;
    }

    private void AddToken(TokenType type)
    {
        AddToken(type, null);
    }

    private void AddToken(TokenType type, object? literal)
    {
        string text = _source[_start.._current];

        _tokens.Add(
            new Token(type, text, literal, _line)
        );
    }
}