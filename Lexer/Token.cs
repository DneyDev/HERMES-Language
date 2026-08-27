namespace HERMESLANG.Lexer;

public class Token
{
    public TokenType Type { get; }
    public string Lexeme { get; }
    public object? Literal { get; }
    public int Line { get; }

    public Token(TokenType type, string lexeme, object? literal, int line)
    {
        Type = type;
        Lexeme = lexeme;
        Literal = literal;
        Line = line;
    }

    public override string ToString()
    {
        if (
            Type == TokenType.NewLine ||
            Type == TokenType.Indent ||
            Type == TokenType.Dedent ||
            Type == TokenType.EndOfFile
            )
        {
            return Type.ToString();
        }
        if(Literal is null)
        {
            return $"{Type} | {Lexeme}";
        } 
        return $"{Type} | {Lexeme} | {Literal}"; 
    }
}