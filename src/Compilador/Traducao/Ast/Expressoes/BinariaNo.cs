// Usado tanto para o fold de OUTROS_TERMOS (soma/sub) quanto de MAIS_FATORES (mult/div).
class BinariaNo : ExpressaoNo
{
    public OperadorBinario Operador { get; }
    public ExpressaoNo Esquerda { get; }
    public ExpressaoNo Direita { get; }

    public BinariaNo(OperadorBinario operador, ExpressaoNo esquerda, ExpressaoNo direita)
    {
        Operador = operador;
        Esquerda = esquerda;
        Direita = direita;
    }

    public override void Aceitar(IVisitorAst visitor) => visitor.Visitar(this);
}