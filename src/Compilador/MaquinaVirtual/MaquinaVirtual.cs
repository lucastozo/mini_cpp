class MaquinaVirtual
{
    private readonly List<Instrucao> Programa = new();
    private readonly Stack<double> Dados = new();

    public void Executar(string programaPath = "Compilador/programa" )
    {
        string[] linhas = File.ReadAllLines(programaPath);
        foreach (string linha in linhas)
        {
            string[] partes = linha.Split(' ');            
            if (!Enum.TryParse(partes[0], out Operacao operacao))
                throw new Exception($"Operação {partes[0]} não existe.");

            double? operando = null;
            if (partes.Length == 2) 
                operando = double.Parse(partes[1], System.Globalization.CultureInfo.InvariantCulture); // InvariantCulture -> . como separador decimal

            Instrucao instrucao = new(operacao, operando);
            Programa.Add(instrucao);
        }

        if(Programa[0].Opcode != Operacao.INPP) throw new Exception("Programa deveria começar com INPP.");
        int PC = 0;

        while (true)
        {
            Instrucao instrucao = Programa[PC];
            switch (instrucao.Opcode)
            {
                case Operacao.INPP:
                    Dados.Clear();
                    break;

                case Operacao.CRCT:
                    Dados.Push(instrucao.Operando!.Value);
                    break;

                case Operacao.CRVL:
                    Dados.Push(Dados.ElementAt((int)instrucao.Operando!.Value));
                    break;

                case Operacao.SOMA:
                    Dados.Push(Dados.Pop() + Dados.Pop());
                    break;

                case Operacao.SUBT:
                {
                    double b = Dados.Pop(); double a = Dados.Pop();
                    Dados.Push(a - b);
                    break;
                }
                
                case Operacao.MULT:
                    Dados.Push(Dados.Pop() * Dados.Pop());
                    break;
                
                case Operacao.DIVI:
                {
                    double b = Dados.Pop(); double a = Dados.Pop();
                    Dados.Push(a / b);
                    break;
                }

                case Operacao.INVE:
                    Dados.Push(-Dados.Pop());
                    break;

                case Operacao.CONJ:
                {
                    double b = Dados.Pop(); double a = Dados.Pop();
                    Dados.Push(a == 1 && b == 1 ? 1 : 0);
                    break;
                }
                
                case Operacao.DISJ:
                {
                    double b = Dados.Pop(); double a = Dados.Pop();
                    Dados.Push(a == 1 || b == 1 ? 1 : 0);
                    break;
                }

                case Operacao.NEGA:
                    Dados.Push(Dados.Pop() == 1 ? 0 : 1);
                    break;

                case Operacao.CPME:
                {
                    double b = Dados.Pop(); double a = Dados.Pop();
                    Dados.Push(a < b ? 1 : 0);
                    break;
                }

                case Operacao.CPMA:
                {
                    double b = Dados.Pop(); double a = Dados.Pop();
                    Dados.Push(a > b ? 1 : 0);
                    break;
                }

                case Operacao.CPIG:
                    Dados.Push(Dados.Pop() == Dados.Pop() ? 1 : 0);
                    break;

                case Operacao.CDES:
                    Dados.Push(Dados.Pop() != Dados.Pop() ? 1 : 0);
                    break;

                case Operacao.CPMI:
                {
                    double b = Dados.Pop(); double a = Dados.Pop();
                    Dados.Push(a <= b ? 1 : 0);
                    break;
                }

                case Operacao.CMAI:
                {
                    double b = Dados.Pop(); double a = Dados.Pop();
                    Dados.Push(a >= b ? 1 : 0);
                    break;
                }

                case Operacao.ARMZ:
                    DadosInserirNoIndice((int)instrucao.Operando!.Value, Dados.Pop());
                    break;

                case Operacao.DSVI:
                    PC = (int)instrucao.Operando!.Value;
                    continue;
                
                case Operacao.DSVF:
                    if (Dados.Pop() == 0) 
                    {
                        PC = (int)instrucao.Operando!.Value;
                        continue;
                    }
                    break;

                case Operacao.LEIT:
                    Dados.Push(double.Parse(Console.ReadLine()!, System.Globalization.CultureInfo.InvariantCulture));
                    break;

                case Operacao.IMPR:
                    Console.WriteLine(Dados.Pop());
                    break;

                case Operacao.PARA:
                    return;
            }

            PC++;
        }
    }

    private void DadosInserirNoIndice(int indice, double valor)
    {
        Stack<double> temp = new();
        
        for (int i = 0; i < indice; i++)
            temp.Push(Dados.Pop());

        Dados.Push(valor);

        while (temp.Count > 0)
            Dados.Push(temp.Pop());
    }
}