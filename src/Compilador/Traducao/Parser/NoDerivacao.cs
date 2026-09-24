/// <summary>
/// Um nó da parse tree.
/// <para>- Nó terminal: representa um token do código-fonte (folha).</para>
/// <para>- Nó não-terminal: representa uma produção aplicada. Os filhos dela são os símbolos do lado direito dessa produção.</para>
/// </summary>
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
}