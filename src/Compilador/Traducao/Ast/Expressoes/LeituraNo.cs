// EXP_IDENT -> LER_DOUBLE ESQPAREN DIRPAREN
// "ler_double()" é uma expressão: produz um valor vindo da entrada.
class LeituraNo : ExpressaoNo
{
    public override void Aceitar(IVisitorAst visitor) => visitor.Visitar(this);
}