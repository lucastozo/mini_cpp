class Parser
{
    private readonly List<Token> _tokens;
    private int _pos;
    
    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    /// <summary>
    /// Valida os tokens usando a Tabela M e devolve a árvore de derivação (raiz = PROG).
    /// </summary>
    public NoDerivacao Parsear()
    {
        NoDerivacao raiz = new(NaoTerminal.PROG);
        NoDerivacao noEof = new(TipoToken.EOF);

        Stack<NoDerivacao> pilha = new();
        pilha.Push(noEof);
        pilha.Push(raiz);

        while (pilha.Count > 0)
        {
            NoDerivacao no = pilha.Pop();
            Token atual = _tokens[_pos];

            switch (no.Simbolo)
            {
                case Terminal terminal:
                    if (terminal.Tipo != atual.Tipo)
                        throw new Exception($"Esperado '{terminal.Tipo}', encontrado '{atual.Tipo}' (lexema '{atual.Lexema}').");
                    
                    no.Token = atual; // folha guarda token que casou
                    _pos++;
                    break;

                case NaoTerminalSimbolo naoTerminalSimbolo:
                    if (!TabelaM.TryConsultar(naoTerminalSimbolo.Tipo, atual.Tipo, out Producao producao))
                        throw new Exception($"Token inesperado '{atual.Tipo}' (lexema '{atual.Lexema}') ao processar '{naoTerminalSimbolo.Tipo}'.");

                    foreach (Simbolo simbolo in producao)
                        no.Filhos.Add(new NoDerivacao(simbolo));

                    for (int i = no.Filhos.Count - 1; i >= 0; i--)
                        pilha.Push(no.Filhos[i]);
                    break;
            }
        }

        return raiz;
    }
}