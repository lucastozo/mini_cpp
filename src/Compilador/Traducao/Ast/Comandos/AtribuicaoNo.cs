// CMD -> ID RESTO_IDENT, com RESTO_IDENT -> ATRIBUICAO EXP_IDENT
class AtribuicaoNo : ComandoNo
{
    public string NomeVariavel { get; }
    public ExpressaoNo Valor { get; } // LiteralNo, IdentificadorNo, BinariaNo, UnariaNo ou LeituraNo

    public AtribuicaoNo(string nomeVariavel, ExpressaoNo valor)
    {
        NomeVariavel = nomeVariavel;
        Valor = valor;
    }

    public override void Aceitar(IVisitorAst visitor) => visitor.Visitar(this);
}