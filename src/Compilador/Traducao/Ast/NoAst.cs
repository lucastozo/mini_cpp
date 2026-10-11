abstract class NoAst
{
    public abstract void Aceitar(IVisitorAst visitor);
}

abstract class ComandoNo : NoAst { }
abstract class ExpressaoNo : NoAst { }