// CMD_COND -> IF ESQPAREN CONDICAO DIRPAREN ESQCHAVE CMDS DIRCHAVE PFALSA
class SeNo : ComandoNo
{
    public CondicaoNo Condicao { get; }
    public List<ComandoNo> Entao { get; }
    public List<ComandoNo>? Senao { get; } // null quando PFALSA é ε

    public SeNo(CondicaoNo condicao, List<ComandoNo> entao, List<ComandoNo>? senao)
    {
        Condicao = condicao;
        Entao = entao;
        Senao = senao;
    }

    public override void Aceitar(IVisitorAst visitor) => visitor.Visitar(this);
}