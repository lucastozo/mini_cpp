// PROG -> ... CMDS DIRCHAVE
class ProgramaNo : NoAst
{
    public List<ComandoNo> Comandos { get; }
    public ProgramaNo(List<ComandoNo> comandos) => Comandos = comandos;

    public override void Aceitar(IVisitorAst visitor) => visitor.Visitar(this);
}