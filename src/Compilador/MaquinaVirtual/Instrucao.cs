public enum Operacao
{
    INPP,
    CRCT,
    IMPR,
    PARA
}

class Instrucao
{
    public Operacao Opcode { get; init; }
    public double? Operando { get; init; }

    public Instrucao(Operacao opcode, double? operando)
    {
        Opcode = opcode;
        Operando = operando;
    }  
}