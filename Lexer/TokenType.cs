namespace HERMESLANG.Lexer;

public enum TokenType
{
    // Valores
    Number,
    String,
    Boolean,

    // Identificadores
    Identifier,

    // Palavras-chave
    Se,
    Senao,
    E,
    Ou,
    Nao,
    Enquanto,

    // Operadores matemáticos
    Plus,
    Minus,
    Multiply,
    Divide,

    // Operadores de comparação
    EqualEqual,
    NotEqual,
    Greater,
    Less,
    GreaterEqual,
    LessEqual,

    // Atribuição
    Assign,

    // Símbolos
    LeftParenthesis,
    RightParenthesis,
    Colon,
    Comma,

    // Controle
    NewLine,
    Indent,
    Dedent,

    EndOfFile
}