class Instrucao
{
    public Operacao Opcode { get; init; }
    public double? Operando { get; init; }

    public Instrucao(Operacao opcode, double? operando)
    {
        Opcode = opcode;
        Operando = operando;
    }

    public override string ToString()
    {
        return Operando != null ? $"{Opcode} {Operando}" : $"{Opcode}";
    }
}

enum Operacao
{
    CRCT,
    CRVL,
    SOMA,
    SUBT,
    MULT,
    DIVI,
    INVE,
    CONJ,
    DISJ,
    NEGA,
    CPME,
    CPMA,
    CPIG,
    CDES,
    CPMI,
    CMAI,
    ARMZ,
    DSVI,
    DSVF,
    LEIT,
    IMPR,
    ALME,
    PARA,
    INPP
}