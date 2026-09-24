class Parser
{
    private readonly List<Token> _tokens;
    private int _pos;
    
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
                case Terminal terminal:
                    if (terminal.Tipo != atual.Tipo)
                        throw new Exception($"Esperado '{terminal.Tipo}', encontrado '{atual.Tipo}' (lexema '{atual.Lexema}').");
                    _pos++;
                    break;

                case NaoTerminalSimbolo naoTerminalSimbolo:
                    if (!TabelaM.TryConsultar(naoTerminalSimbolo.Tipo, atual.Tipo, out Producao producao))
                        throw new Exception($"Token inesperado '{atual.Tipo}' (lexema '{atual.Lexema}') ao processar '{naoTerminalSimbolo.Tipo}'.");

                    for (int i = producao.Count - 1; i >= 0; i--)
                        pilha.Push(producao[i]);
                    break;
            }
        }
    }
}