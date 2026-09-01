if (args.Length != 2 || args[0] != "-g" && args[0] != "-e")
{
    Console.WriteLine("Uso:");
    Console.WriteLine("  Compilador -g <caminho_codigo_fonte>");
    Console.WriteLine("      Gera código objeto.");
    Console.WriteLine("      Saída: <nome>.out na pasta atual (pwd).");
    Console.WriteLine();
    Console.WriteLine("  Compilador -e <caminho_codigo_objeto>");
    Console.WriteLine("      Executa o código objeto.");
    Environment.Exit(-1);
}

string codigo_fonte = File.ReadAllText(args[1]);