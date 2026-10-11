// CONDICAO -> EXPRESSAO RELACAO EXPRESSAO
class CondicaoNo : NoAst
{
    public ExpressaoNo Esquerda { get; }
    public OperadorRelacional Operador { get; }
    public ExpressaoNo Direita { get; }

    public CondicaoNo(ExpressaoNo esquerda, OperadorRelacional operador, ExpressaoNo direita)
    {
        Esquerda = esquerda;
        Operador = operador;
        Direita = direita;
    }

    public override void Aceitar(IVisitorAst visitor) => visitor.Visitar(this);
}