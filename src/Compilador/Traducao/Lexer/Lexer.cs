class Lexer
{
    private readonly string _codigoFonte;
    private int _pos;

    private static readonly Dictionary<string, TipoToken> PalavrasReservadas = new()
    {
        { "include", TipoToken.INCLUDE },
        { "iostream", TipoToken.IOSTREAM },
        { "using", TipoToken.USING },
        { "namespace", TipoToken.NAMESPACE },
        { "std", TipoToken.STD },
        { "int", TipoToken.INT },
        { "main", TipoToken.MAIN },
        { "double", TipoToken.DOUBLE },
        { "if", TipoToken.IF },
        { "else", TipoToken.ELSE },
        { "while", TipoToken.WHILE },
        { "cout", TipoToken.COUT },
        { "endl", TipoToken.ENDL },
        { "lerDouble", TipoToken.LER_DOUBLE },
    };

    public Lexer(string codigoFonte)
    {
        _codigoFonte = codigoFonte;
    }

    public List<Token> Tokenizar()
    {
        List<Token> tokens = new();

        while (_pos < _codigoFonte.Length)
        {
            char atual = _codigoFonte[_pos];

            if (char.IsWhiteSpace(atual))
            {
                _pos++;
            }
            else if (char.IsLetter(atual))
            {
                tokens.Add(LerIdentificadorOuPalavraReservada());
            }
            else if (char.IsDigit(atual))
            {
                tokens.Add(LerNumero());
            }
            else
            {
                tokens.Add(LerSimboloOuOperador());
            }
        }

        tokens.Add(new Token("", TipoToken.EOF));
        return tokens;
    }

    private Token LerIdentificadorOuPalavraReservada()
    {
        int inicio = _pos;

        while (_pos < _codigoFonte.Length && char.IsLetterOrDigit(_codigoFonte[_pos]))
            _pos++;

        string lexema = _codigoFonte.Substring(inicio, _pos - inicio);

        if (PalavrasReservadas.TryGetValue(lexema, out TipoToken tipo))
            return new Token(lexema, tipo);

        return new Token(lexema, TipoToken.ID);
    }

    private Token LerNumero()
    {
        int inicio = _pos;

        while (_pos < _codigoFonte.Length && char.IsDigit(_codigoFonte[_pos]))
            _pos++;
        
        if (_pos < _codigoFonte.Length && _codigoFonte[_pos] == '.')
        {
            _pos++;
            while (_pos < _codigoFonte.Length && char.IsDigit(_codigoFonte[_pos]))
                _pos++;
        }

        string lexema = _codigoFonte.Substring(inicio, _pos - inicio);
        return new Token(lexema, TipoToken.NUMERO_REAL);
    }

    private Token LerSimboloOuOperador()
    {
        char atual = _codigoFonte[_pos];
        _pos++;

        switch (atual)
        {
            case '#': return new Token("#", TipoToken.CERQUILHA);
            case ';': return new Token(";", TipoToken.PONTOVIRGULA);
            case ',': return new Token(",", TipoToken.VIRGULA);
            case '(': return new Token("(", TipoToken.ESQPAREN);
            case ')': return new Token(")", TipoToken.DIRPAREN);
            case '{': return new Token("{", TipoToken.ESQCHAVE);
            case '}': return new Token("}", TipoToken.DIRCHAVE);
            case '+': return new Token("+", TipoToken.MAIS);
            case '-': return new Token("-", TipoToken.MENOS);
            case '*': return new Token("*", TipoToken.MULT);
            case '/': return new Token("/", TipoToken.DIV);

            case '=':
                if (_pos < _codigoFonte.Length && _codigoFonte[_pos] == '=')
                {
                    _pos++;
                    return new Token("==", TipoToken.IG);
                }
                return new Token("=", TipoToken.ATRIBUICAO);

            case '!':
                if (_pos < _codigoFonte.Length && _codigoFonte[_pos] == '=')
                {
                    _pos++;
                    return new Token("!=", TipoToken.DES);
                }
                throw new Exception("Esperado '=' após '!'.");

            case '>':
                if (_pos < _codigoFonte.Length && _codigoFonte[_pos] == '=')
                {
                    _pos++;
                    return new Token(">=", TipoToken.MAI);
                }
                return new Token(">", TipoToken.MA);

            case '<':
                if (_pos < _codigoFonte.Length && _codigoFonte[_pos] == '<')
                {
                    _pos++;
                    return new Token("<<", TipoToken.INSERCAO);
                }
                if (_pos < _codigoFonte.Length && _codigoFonte[_pos] == '=')
                {
                    _pos++;
                    return new Token("<=", TipoToken.MI);
                }
                return new Token("<", TipoToken.ME);

            default:
                throw new Exception($"Caractere inesperado '{atual}'.");
        }
    }
}