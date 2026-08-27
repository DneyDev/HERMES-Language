using HERMESLANG.Lexer;
using HERMESLANG.Parser;

string codigo = """
vida = 100

se vida > 50:
    mostrar("Vida alta")
""";

Lexer lexer = new Lexer(codigo);

List<Token> tokens = lexer.ScanTokens();

Parser parser = new Parser(tokens);

try
{
    List<Statement> statements = parser.Parse();

    Console.WriteLine("Parser executado com sucesso!");
    Console.WriteLine($"Statements encontrados: {statements.Count}");
}
catch (ParserException ex)
{
    Console.WriteLine($"Erro de Parser: {ex.Message}");
}