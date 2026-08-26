using HERMESLANG.Lexer;

class Program
{
    static void Main()
    {
        string codigo = """
vida = 100

se vida > 50:
    mostrar("Vida alta")
""";

        Lexer lexer = new Lexer(codigo);

        List<Token> tokens = lexer.ScanTokens();

        foreach (Token token in tokens)
        {
            Console.WriteLine(token);
        }
    }
}