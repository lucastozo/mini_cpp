// CMD_COND -> WHILE ESQPAREN CONDICAO DIRPAREN ESQCHAVE CMDS DIRCHAVE
class EnquantoNo : ComandoNo
{
    public CondicaoNo Condicao { get; }
    public List<ComandoNo> Corpo { get; }

    public EnquantoNo(CondicaoNo condicao, List<ComandoNo> corpo)
    {
        Condicao = condicao;
        Corpo = corpo;
    }

    public override void Aceitar(IVisitorAst visitor) => visitor.Visitar(this);
}