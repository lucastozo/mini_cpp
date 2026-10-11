class IdentificadorNo : ExpressaoNo
{
    public string Nome { get; }
    public IdentificadorNo(string nome) => Nome = nome;

    public override void Aceitar(IVisitorAst visitor) => visitor.Visitar(this);
}