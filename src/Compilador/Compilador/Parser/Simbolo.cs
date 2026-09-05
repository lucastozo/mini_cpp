global using Producao = System.Collections.Generic.List<Simbolo>;

abstract record Simbolo
{
    public static implicit operator Simbolo(TipoToken tipo) => new Terminal(tipo);
    public static implicit operator Simbolo(NaoTerminal naoTerminal) => new NaoTerminalSimbolo(naoTerminal);
}

sealed record Terminal(TipoToken Tipo) : Simbolo;
sealed record NaoTerminalSimbolo(NaoTerminal Tipo) : Simbolo;