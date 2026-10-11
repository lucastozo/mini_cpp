class NoDerivacao
{
    public Simbolo Simbolo { get; }
    public Token? Token { get; set; }
    public List<NoDerivacao> Filhos { get; } = new();

    public NoDerivacao(Simbolo simbolo)
    {
        Simbolo = simbolo;
    }

    public bool EhVazio => Simbolo is NaoTerminalSimbolo && Filhos.Count == 0;

    public NoDerivacao Filho(NaoTerminal tipo)
    {
        foreach (NoDerivacao filho in Filhos)
            if (filho.Simbolo is NaoTerminalSimbolo nts && nts.Tipo == tipo)
                return filho;

        throw new InvalidOperationException($"Nenhum filho '{tipo}' encontrado em '{Simbolo}'.");
    }

    public Token FilhoToken(TipoToken tipo)
    {
        foreach (NoDerivacao filho in Filhos)
            if (filho.Simbolo is Terminal t && t.Tipo == tipo)
                return filho.Token!;

        throw new InvalidOperationException($"Nenhum filho '{tipo}' encontrado em '{Simbolo}'.");
    }
}