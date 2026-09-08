class Parser
{
    private readonly List<Token> _tokens;
    private int _pos;

    // Ferramentas que as ações (definidas na TabelaM) chamam.
    public TabelaDeSimbolos TabelaDeSimbolos { get; } = new();
    public TabelaDeEnderecos TabelaDeEnderecos { get; } = new();
    public GeradorDeCodigo Gerador { get; } = new();

    public Token TokenAtual => _tokens[_pos];

    // --- Estado auxiliar exigido pelas ações do code gen ---

    // Pendentes que precisam de pilha (a regra pode reentrar pelo parenteses):
    public Stack<Operacao> PilhaOperadorAditivo { get; } = new(); // OP_AD dentro de OUTROS_TERMOS
    public Stack<Operacao> PilhaOperadorMultiplicativo { get; } = new(); // OP_MUL dentro de MAIS_FATORES
    public Stack<bool> PilhaUnario { get; } = new(); // OP_UN dentro de TERMO

    // Pendentes de if/while que precisam de pilha por causa de aninhamento:
    public Stack<int> PendentesPuloElse { get; } = new(); // endereço do DSVF a corrigir
    public Stack<int> PendentesPuloFim { get; } = new(); // endereço do DSVI/DSVF a corrigir
    public Stack<int> PilhaInicioLaco { get; } = new(); // endereço de retorno do while

    // Pendentes que a regra nunca reentra em si mesma:
    public Operacao? RelacaoPendente { get; set; }
    public string? AlvoAtribuicaoPendente { get; set; }

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
            Token atual = TokenAtual;

            switch (topo)
            {
                case Terminal(var tipoEsperado):
                    if (tipoEsperado != atual.Tipo)
                        throw new Exception($"Esperado {tipoEsperado}, encontrado {atual.Tipo} (lexema '{atual.Lexema}').");

                    _pos++;
                    break;

                case NaoTerminalSimbolo(var naoTerminal):
                    if (!TabelaM.TryConsultar(naoTerminal, atual.Tipo, out Producao? producao))
                        throw new Exception($"Token inesperado {atual.Tipo} (lexema '{atual.Lexema}') ao processar {naoTerminal}.");

                    for (int i = producao!.Count - 1; i >= 0; i--)
                        pilha.Push(producao[i]);
                    break;

                case Acao(var executar):
                    executar(this);
                    break;
            }
        }
    }
}