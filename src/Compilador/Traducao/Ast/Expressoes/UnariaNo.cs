// TERMO -> OP_UN FATOR ... (quando OP_UN não é ε)
class UnariaNo : ExpressaoNo
{
    public OperadorUnario Operador { get; }
    public ExpressaoNo Operando { get; }

    public UnariaNo(OperadorUnario operador, ExpressaoNo operando)
    {
        Operador = operador;
        Operando = operando;
    }

    public override void Aceitar(IVisitorAst visitor) => visitor.Visitar(this);
}