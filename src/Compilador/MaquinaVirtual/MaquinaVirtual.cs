class MaquinaVirtual
{
    private readonly PilhaDados _dados = new();

    public void Executar(List<Instrucao> programa)
    {
        int PC = 0;
        while (true)
        {
            Instrucao instrucao = programa[PC];
            switch (instrucao.Opcode)
            {
                case Operacao.INPP:
                    _dados.Clear();
                    break;

                case Operacao.CRCT:
                    _dados.Push(instrucao.Operando!.Value);
                    break;

                case Operacao.CRVL:
                    _dados.Push(_dados[(int)instrucao.Operando!.Value]);
                    break;

                case Operacao.SOMA:
                    _dados.Push(_dados.Pop() + _dados.Pop());
                    break;

                case Operacao.SUBT:
                {
                    double b = _dados.Pop(); double a = _dados.Pop();
                    _dados.Push(a - b);
                    break;
                }
                
                case Operacao.MULT:
                    _dados.Push(_dados.Pop() * _dados.Pop());
                    break;
                
                case Operacao.DIVI:
                {
                    double b = _dados.Pop(); double a = _dados.Pop();
                    _dados.Push(a / b);
                    break;
                }

                case Operacao.INVE:
                    _dados.Push(-_dados.Pop());
                    break;

                case Operacao.CONJ:
                {
                    double b = _dados.Pop(); double a = _dados.Pop();
                    _dados.Push(a == 1 && b == 1 ? 1 : 0);
                    break;
                }
                
                case Operacao.DISJ:
                {
                    double b = _dados.Pop(); double a = _dados.Pop();
                    _dados.Push(a == 1 || b == 1 ? 1 : 0);
                    break;
                }

                case Operacao.NEGA:
                    _dados.Push(_dados.Pop() == 1 ? 0 : 1);
                    break;

                case Operacao.CPME:
                {
                    double b = _dados.Pop(); double a = _dados.Pop();
                    _dados.Push(a < b ? 1 : 0);
                    break;
                }

                case Operacao.CPMA:
                {
                    double b = _dados.Pop(); double a = _dados.Pop();
                    _dados.Push(a > b ? 1 : 0);
                    break;
                }

                case Operacao.CPIG:
                    _dados.Push(_dados.Pop() == _dados.Pop() ? 1 : 0);
                    break;

                case Operacao.CDES:
                    _dados.Push(_dados.Pop() != _dados.Pop() ? 1 : 0);
                    break;

                case Operacao.CPMI:
                {
                    double b = _dados.Pop(); double a = _dados.Pop();
                    _dados.Push(a <= b ? 1 : 0);
                    break;
                }

                case Operacao.CMAI:
                {
                    double b = _dados.Pop(); double a = _dados.Pop();
                    _dados.Push(a >= b ? 1 : 0);
                    break;
                }

                case Operacao.ARMZ:
                    _dados[(int)instrucao.Operando!.Value] = _dados.Pop();
                    break;

                case Operacao.DSVI:
                    PC = (int)instrucao.Operando!.Value;
                    continue;
                
                case Operacao.DSVF:
                    if (_dados.Pop() == 0) 
                    {
                        PC = (int)instrucao.Operando!.Value;
                        continue;
                    }
                    break;

                case Operacao.LEIT:
                    _dados.Push(double.Parse(Console.ReadLine()!, System.Globalization.CultureInfo.InvariantCulture));
                    break;

                case Operacao.IMPR:
                    Console.WriteLine(_dados.Pop());
                    break;
                
                case Operacao.ALME:
                    for (int i = 0; i < (int)instrucao.Operando!.Value; i++)
                        _dados.Push(double.NaN);
                    break;

                case Operacao.PARA:
                    return;
            }

            PC++;
        }
    }
}