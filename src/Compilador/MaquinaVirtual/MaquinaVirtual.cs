class MaquinaVirtual
{
    private List<Instrucao> Programa = new();
    private Stack<double> Dados = new();

    public void Executar(string programaPath = "Compilador/programa" )
    {
        string[] linhas = File.ReadAllLines(programaPath);
        foreach (string linha in linhas)
        {
            string[] partes = linha.Split(' ');
            if (partes.Length > 2) throw new Exception($"Formato de instrução inválido.");

            if (!Enum.TryParse(partes[0], out Operacao operacao))
                throw new Exception($"Operação {partes[0]} não existe.");

            double? operando = null;
            if (partes.Length == 2) 
                operando = double.Parse(partes[1], System.Globalization.CultureInfo.InvariantCulture); // InvariantCulture -> . como separador decimal

            Instrucao instrucao = new(operacao, operando);
            Programa.Add(instrucao);
        }

        if(Programa[0].Opcode != Operacao.INPP) throw new Exception("Programa deveria começar com INPP.");
        int i = 0;

        while (true)
        {
            Instrucao instrucao = Programa[i];
            switch (instrucao.Opcode)
            {
                case Operacao.INPP:
                    Dados.Clear();
                    break;

                case Operacao.CRCT:
                    Dados.Push(instrucao.Operando!.Value);
                    break;

                case Operacao.IMPR:
                    Console.WriteLine(Dados.Pop());
                    break;

                case Operacao.PARA:
                    return;
            }

            i++;
        }
    }
}