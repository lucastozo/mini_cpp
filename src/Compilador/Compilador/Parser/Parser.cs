class Parser
{
    private readonly List<Token> _tokens;
    private int _pos;
    private readonly TabelaDeSimbolos _tabelaDeSimbolos = new();
    private readonly Stack<TipoAcaoSemantica> _acoesPendentes = new();

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public void Parsear()
    {
        Stack<Simbolo> pilha = new();
        pilha.Push(TipoToken.EOF);
        pilha.Push(NaoTerminal.PROG);

        while (pilha.Count > 0)
        {
            Simbolo topo = pilha.Pop();
            Token atual = _tokens[_pos];

            switch (topo)
            {
                case Terminal(var tipoEsperado):
                    if (tipoEsperado == TipoToken.ID && _acoesPendentes.Count > 0)
                    {
                        TipoAcaoSemantica acao = _acoesPendentes.Pop();
                        if (acao == TipoAcaoSemantica.Declarar)
                            _tabelaDeSimbolos.Declarar(atual.Lexema);
                        else
                            _tabelaDeSimbolos.Verificar(atual.Lexema);
                    }
                    
                    if (tipoEsperado != atual.Tipo)
                        throw new Exception($"Esperado {tipoEsperado}, encontrado {atual.Tipo} (lexema '{atual.Lexema}').");

                    _pos++;
                    break;
                case NaoTerminalSimbolo(var naoTerminal):
                    if (!TabelaM.TryConsultar(naoTerminal, atual.Tipo, out Producao? producao))
                        throw new Exception($"Token inesperado {atual.Tipo} (lexema '{atual.Lexema}') ao processar {naoTerminal}.");
                    
                    for (int i = producao!.Count - 1; i >= 0; i--)
                        pilha.Push(producao[i]);

                    if (GatilhosSemanticos.TryConsultar(naoTerminal, atual.Tipo, out TipoAcaoSemantica acaoAgendada))
                        _acoesPendentes.Push(acaoAgendada);
                        
                    break;
            }
        }
    }
}