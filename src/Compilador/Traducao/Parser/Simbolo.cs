global using Producao = System.Collections.Generic.List<Simbolo>;

abstract class Simbolo
{
    public static implicit operator Simbolo(TipoToken tipo) => new Terminal(tipo);
    public static implicit operator Simbolo(NaoTerminal naoTerminal) => new NaoTerminalSimbolo(naoTerminal);
}

class Terminal : Simbolo
{
    public TipoToken Tipo { get; }
    public Terminal(TipoToken tipo)
    {
        Tipo = tipo;
    }
}

class NaoTerminalSimbolo : Simbolo
{
    public NaoTerminal Tipo { get; }
    public NaoTerminalSimbolo(NaoTerminal tipo)
    {
        Tipo = tipo;
    }
}