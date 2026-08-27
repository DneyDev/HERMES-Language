using HERMESLANG.Lexer;
using HERMESLANG.Parser;
using HERMESLANG.Interpreter;

string codigo = """
vida = 100
outra_vida = vida
""";

Lexer lexer = new Lexer(codigo);

List<Token> tokens = lexer.ScanTokens();

Parser parser = new Parser(tokens);

try
{
    List<Statement> statements = parser.Parse();

    Console.WriteLine("=== AST ===");

    ASTPrinter printer = new ASTPrinter();
    printer.Print(statements);

    Console.WriteLine();
    Console.WriteLine("=== INTERPRETER ===");

    Interpreter interpreter = new Interpreter();

    interpreter.Interpret(statements);
    foreach (var variable in interpreter.GetVariables())
    {
        Console.WriteLine(
            $"{variable.Key} = {variable.Value}"
        );
    }

    Console.WriteLine("Programa executado com sucesso!");
}
catch (ParserException ex)
{
    Console.WriteLine($"Erro de Parser: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro de execução: {ex.Message}");
}