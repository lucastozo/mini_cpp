enum TipoAcaoSemantica
{
    Declarar,
    Verificar
}

static class GatilhosSemanticos
{
    // só os 4 pares ordenados que um ID aparece:
    // VARS -> ID MAIS_VAR 
    // CMD -> ID RESTO_IDENT
    // CMD -> COUT INSERCAO ID...
    // FATOR -> ID
    private static readonly Dictionary<(NaoTerminal, TipoToken), TipoAcaoSemantica> _gatilhos = new()
    {
        { (NaoTerminal.VARS, TipoToken.ID), TipoAcaoSemantica.Declarar },
        { (NaoTerminal.CMD, TipoToken.ID), TipoAcaoSemantica.Verificar },
        { (NaoTerminal.CMD, TipoToken.COUT), TipoAcaoSemantica.Verificar },
        { (NaoTerminal.FATOR, TipoToken.ID), TipoAcaoSemantica.Verificar },
    };

    public static bool TryConsultar(NaoTerminal naoTerminal, TipoToken token, out TipoAcaoSemantica acao)
    {
        return _gatilhos.TryGetValue((naoTerminal, token), out acao);
    }
}