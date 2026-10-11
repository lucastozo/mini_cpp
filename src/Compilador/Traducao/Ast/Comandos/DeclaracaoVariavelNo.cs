// VAR -> TIPO VARS | VARS -> ID MAIS_VAR
// Toda variável é double, então o único dado relevante é a lista de nomes.
class DeclaracaoVariavelNo : ComandoNo
{
    public List<string> Nomes { get; }
    public DeclaracaoVariavelNo(List<string> nomes) => Nomes = nomes;

    public override void Aceitar(IVisitorAst visitor) => visitor.Visitar(this);
}