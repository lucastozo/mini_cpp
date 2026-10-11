// CMD -> COUT INSERCAO ID INSERCAO ENDL
// A gramática só aceita um ID aqui, por isso o campo é string, não ExpressaoNo.
class EscritaNo : ComandoNo
{
    public string NomeVariavel { get; }
    public EscritaNo(string nomeVariavel) => NomeVariavel = nomeVariavel;

    public override void Aceitar(IVisitorAst visitor) => visitor.Visitar(this);
}