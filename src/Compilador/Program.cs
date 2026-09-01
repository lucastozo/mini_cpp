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
if (gerar)
{
    string codigoFonte = File.ReadAllText(args[1]);
}
else
{
    string[] codigoObjeto = File.ReadAllLines(args[1]);
    List<Instrucao> programa = MontadorDeInstrucoes.Montar(codigoObjeto);
    MaquinaVirtual vm = new();
    vm.Executar(programa);
}