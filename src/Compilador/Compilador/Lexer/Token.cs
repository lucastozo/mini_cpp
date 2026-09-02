class Token
{
    public string Lexema { get; init; }
    public TipoToken Tipo { get; init; }

    public Token(string lexema, TipoToken tipo)
    {
        Lexema = lexema;
        Tipo = tipo;
    }
}

enum TipoToken
{
    INCLUDE, IOSTREAM, USING, NAMESPACE, STD, INT, MAIN,
    DOUBLE, IF, ELSE, WHILE, COUT, ENDL, LER_DOUBLE,

    ID, NUMERO_REAL,

    ATRIBUICAO, // =
    MAIS, // + 
    MENOS, // - 
    MULT, // *
    DIV, // /
    IG, // == 
    DES, // !=
    MA, // >
    ME, // < 
    MAI, // >=
    MI, // <=
    INSERCAO, // << (para usar com o cout)

    CERQUILHA,
    PONTOVIRGULA,
    VIRGULA,
    ESQPAREN, DIRPAREN,
    ESQCHAVE, DIRCHAVE,

    EOF
}