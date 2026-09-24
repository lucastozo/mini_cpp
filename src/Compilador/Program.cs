if (args.Length != 2 || (args[0] != "-g" && args[0] != "-e"))
{
    Console.WriteLine("Uso:");
    Console.WriteLine("  Compilador -g <caminho_codigo_fonte>");
    Console.WriteLine("      Gera código objeto.");
    Console.WriteLine("      Saída: <nome>.out na pasta atual (pwd).");
    Console.WriteLine("  Compilador -e <caminho_codigo_objeto>");
    Console.WriteLine("      Executa o código objeto.");
    Environment.Exit(-1);
}

bool gerar = args[0] == "-g";
try
{
    if (gerar)
    {
        string codigoFonte = File.ReadAllText(args[1]);
        Lexer lexer = new(codigoFonte);
        List<Token> tokens = lexer.Tokenizar();
        
        Parser parser = new(tokens);
        NoDerivacao arvore = parser.Parsear();
        
        Console.WriteLine(ImpressoraDerivacao.Gerar(arvore));

        /*
        List<Instrucao> programa = parser.Gerador.ObterPrograma();
        string nomeSaida = Path.ChangeExtension(Path.GetFileName(args[1]), ".out");
        string caminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), nomeSaida);
        using StreamWriter writer = new(caminhoArquivo);
        foreach (Instrucao instrucao in programa)
            writer.WriteLine(instrucao);
        */
    }
    else
    {
        string[] codigoObjeto = File.ReadAllLines(args[1]);
        List<Instrucao> programa = MontadorDeInstrucoes.Montar(codigoObjeto);
        MaquinaVirtual vm = new();
        vm.Executar(programa);
    }
} 
catch (Exception e)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(e.Message);
    Console.ResetColor();
}