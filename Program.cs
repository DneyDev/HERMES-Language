using HERMESLANG.Lexer;
using HERMESLANG.Parser;
using HERMESLANG.Interpreter;
using HERMESLANG.Runtime.Functions;

string codigo = """ 
contador = 0

enquanto contador < 5:
    contador = contador + 1
"""; //essa parte "codigo" é onde aparece no terminal no final dele, onde testa se funciona

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
    //abaixo é onde registra funções
    interpreter.RegisterFunction(
        new MostrarFunction()
    );

    interpreter.RegisterFunction(
        new AbrirPortaFunction()
    );

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