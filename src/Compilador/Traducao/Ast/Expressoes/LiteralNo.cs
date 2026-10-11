class LiteralNo : ExpressaoNo
{
    public double Valor { get; }
    public LiteralNo(double valor) => Valor = valor;

    public override void Aceitar(IVisitorAst visitor) => visitor.Visitar(this);
}