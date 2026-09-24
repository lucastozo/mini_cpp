static class TabelaM
{
    private static readonly Dictionary<(NaoTerminal, TipoToken), Producao> _tabela = new()
    {
        { (NaoTerminal.PROG, TipoToken.CERQUILHA), new() { TipoToken.CERQUILHA, TipoToken.INCLUDE, TipoToken.ME, TipoToken.IOSTREAM, TipoToken.MA, TipoToken.USING, TipoToken.NAMESPACE, TipoToken.STD, TipoToken.PONTOVIRGULA, TipoToken.INT, TipoToken.MAIN, TipoToken.ESQPAREN, TipoToken.DIRPAREN, TipoToken.ESQCHAVE, NaoTerminal.CMDS, TipoToken.DIRCHAVE } },

        { (NaoTerminal.DC, TipoToken.DOUBLE), new() { NaoTerminal.VAR, NaoTerminal.MAIS_CMDS } },

        { (NaoTerminal.VAR, TipoToken.DOUBLE), new() { NaoTerminal.TIPO, NaoTerminal.VARS } },

        { (NaoTerminal.VARS, TipoToken.ID), new() { TipoToken.ID, NaoTerminal.MAIS_VAR } },

        { (NaoTerminal.MAIS_VAR, TipoToken.PONTOVIRGULA), new() },
        { (NaoTerminal.MAIS_VAR, TipoToken.VIRGULA), new() { TipoToken.VIRGULA, NaoTerminal.VARS } },

        { (NaoTerminal.TIPO, TipoToken.DOUBLE), new() { TipoToken.DOUBLE } },

        { (NaoTerminal.CMDS, TipoToken.COUT), new() { NaoTerminal.CMD, NaoTerminal.MAIS_CMDS } },
        { (NaoTerminal.CMDS, TipoToken.DIRCHAVE), new() },
        { (NaoTerminal.CMDS, TipoToken.DOUBLE), new() { NaoTerminal.DC } },
        { (NaoTerminal.CMDS, TipoToken.ID), new() { NaoTerminal.CMD, NaoTerminal.MAIS_CMDS } },
        { (NaoTerminal.CMDS, TipoToken.IF), new() { NaoTerminal.CMD_COND, NaoTerminal.CMDS } },
        { (NaoTerminal.CMDS, TipoToken.WHILE), new() { NaoTerminal.CMD_COND, NaoTerminal.CMDS } },

        { (NaoTerminal.MAIS_CMDS, TipoToken.PONTOVIRGULA), new() { TipoToken.PONTOVIRGULA, NaoTerminal.CMDS } },

        { (NaoTerminal.CMD_COND, TipoToken.IF), new() { TipoToken.IF, TipoToken.ESQPAREN, NaoTerminal.CONDICAO, TipoToken.DIRPAREN, TipoToken.ESQCHAVE, NaoTerminal.CMDS, TipoToken.DIRCHAVE, NaoTerminal.PFALSA } },
        { (NaoTerminal.CMD_COND, TipoToken.WHILE), new() { TipoToken.WHILE, TipoToken.ESQPAREN, NaoTerminal.CONDICAO, TipoToken.DIRPAREN, TipoToken.ESQCHAVE, NaoTerminal.CMDS, TipoToken.DIRCHAVE } },

        { (NaoTerminal.CMD, TipoToken.COUT), new() { TipoToken.COUT, TipoToken.INSERCAO, TipoToken.ID, TipoToken.INSERCAO, TipoToken.ENDL } },
        { (NaoTerminal.CMD, TipoToken.ID), new() { TipoToken.ID, NaoTerminal.RESTO_IDENT } },

        { (NaoTerminal.PFALSA, TipoToken.COUT), new() },
        { (NaoTerminal.PFALSA, TipoToken.DIRCHAVE), new() },
        { (NaoTerminal.PFALSA, TipoToken.DOUBLE), new() },
        { (NaoTerminal.PFALSA, TipoToken.ELSE), new() { TipoToken.ELSE, TipoToken.ESQCHAVE, NaoTerminal.CMDS, TipoToken.DIRCHAVE } },
        { (NaoTerminal.PFALSA, TipoToken.ID), new() },
        { (NaoTerminal.PFALSA, TipoToken.IF), new() },
        { (NaoTerminal.PFALSA, TipoToken.WHILE), new() },

        { (NaoTerminal.RESTO_IDENT, TipoToken.ATRIBUICAO), new() { TipoToken.ATRIBUICAO, NaoTerminal.EXP_IDENT } },

        { (NaoTerminal.EXP_IDENT, TipoToken.ESQPAREN), new() { NaoTerminal.EXPRESSAO } },
        { (NaoTerminal.EXP_IDENT, TipoToken.ID), new() { NaoTerminal.EXPRESSAO } },
        { (NaoTerminal.EXP_IDENT, TipoToken.LER_DOUBLE), new() { TipoToken.LER_DOUBLE, TipoToken.ESQPAREN, TipoToken.DIRPAREN } },
        { (NaoTerminal.EXP_IDENT, TipoToken.MENOS), new() { NaoTerminal.EXPRESSAO } },
        { (NaoTerminal.EXP_IDENT, TipoToken.NUMERO_REAL), new() { NaoTerminal.EXPRESSAO } },

        { (NaoTerminal.CONDICAO, TipoToken.ESQPAREN), new() { NaoTerminal.EXPRESSAO, NaoTerminal.RELACAO, NaoTerminal.EXPRESSAO } },
        { (NaoTerminal.CONDICAO, TipoToken.ID), new() { NaoTerminal.EXPRESSAO, NaoTerminal.RELACAO, NaoTerminal.EXPRESSAO } },
        { (NaoTerminal.CONDICAO, TipoToken.MENOS), new() { NaoTerminal.EXPRESSAO, NaoTerminal.RELACAO, NaoTerminal.EXPRESSAO } },
        { (NaoTerminal.CONDICAO, TipoToken.NUMERO_REAL), new() { NaoTerminal.EXPRESSAO, NaoTerminal.RELACAO, NaoTerminal.EXPRESSAO } },

        { (NaoTerminal.RELACAO, TipoToken.DES), new() { TipoToken.DES } },
        { (NaoTerminal.RELACAO, TipoToken.IG), new() { TipoToken.IG } },
        { (NaoTerminal.RELACAO, TipoToken.MA), new() { TipoToken.MA } },
        { (NaoTerminal.RELACAO, TipoToken.MAI), new() { TipoToken.MAI } },
        { (NaoTerminal.RELACAO, TipoToken.ME), new() { TipoToken.ME } },
        { (NaoTerminal.RELACAO, TipoToken.MI), new() { TipoToken.MI } },

        { (NaoTerminal.EXPRESSAO, TipoToken.ESQPAREN), new() { NaoTerminal.TERMO, NaoTerminal.OUTROS_TERMOS } },
        { (NaoTerminal.EXPRESSAO, TipoToken.ID), new() { NaoTerminal.TERMO, NaoTerminal.OUTROS_TERMOS } },
        { (NaoTerminal.EXPRESSAO, TipoToken.MENOS), new() { NaoTerminal.TERMO, NaoTerminal.OUTROS_TERMOS } },
        { (NaoTerminal.EXPRESSAO, TipoToken.NUMERO_REAL), new() { NaoTerminal.TERMO, NaoTerminal.OUTROS_TERMOS } },
        
        { (NaoTerminal.TERMO, TipoToken.ESQPAREN), new() { NaoTerminal.OP_UN, NaoTerminal.FATOR, NaoTerminal.MAIS_FATORES } },
        { (NaoTerminal.TERMO, TipoToken.ID), new() { NaoTerminal.OP_UN, NaoTerminal.FATOR, NaoTerminal.MAIS_FATORES } },
        { (NaoTerminal.TERMO, TipoToken.MENOS), new() { NaoTerminal.OP_UN, NaoTerminal.FATOR, NaoTerminal.MAIS_FATORES } },
        { (NaoTerminal.TERMO, TipoToken.NUMERO_REAL), new() { NaoTerminal.OP_UN, NaoTerminal.FATOR, NaoTerminal.MAIS_FATORES } },

        { (NaoTerminal.OP_UN, TipoToken.ESQPAREN), new() },
        { (NaoTerminal.OP_UN, TipoToken.ID), new() },
        { (NaoTerminal.OP_UN, TipoToken.MENOS), new() { TipoToken.MENOS } },
        { (NaoTerminal.OP_UN, TipoToken.NUMERO_REAL), new() },

        { (NaoTerminal.FATOR, TipoToken.ESQPAREN), new() { TipoToken.ESQPAREN, NaoTerminal.EXPRESSAO, TipoToken.DIRPAREN } },
        { (NaoTerminal.FATOR, TipoToken.ID), new() { TipoToken.ID } },
        { (NaoTerminal.FATOR, TipoToken.NUMERO_REAL), new() { TipoToken.NUMERO_REAL } },

        { (NaoTerminal.OUTROS_TERMOS, TipoToken.DES), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.DIRPAREN), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.IG), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.MA), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.MAI), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.MAIS), new() { NaoTerminal.OP_AD, NaoTerminal.TERMO, NaoTerminal.OUTROS_TERMOS } },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.ME), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.MENOS), new() { NaoTerminal.OP_AD, NaoTerminal.TERMO, NaoTerminal.OUTROS_TERMOS } },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.MI), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.PONTOVIRGULA), new() },

        { (NaoTerminal.OP_AD, TipoToken.MAIS), new() { TipoToken.MAIS } },
        { (NaoTerminal.OP_AD, TipoToken.MENOS), new() { TipoToken.MENOS } },

        { (NaoTerminal.MAIS_FATORES, TipoToken.DES), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.DIRPAREN), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.DIV), new() { NaoTerminal.OP_MUL, NaoTerminal.FATOR, NaoTerminal.MAIS_FATORES } },
        { (NaoTerminal.MAIS_FATORES, TipoToken.IG), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.MA), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.MAI), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.MAIS), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.ME), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.MENOS), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.MI), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.MULT), new() { NaoTerminal.OP_MUL, NaoTerminal.FATOR, NaoTerminal.MAIS_FATORES } },
        { (NaoTerminal.MAIS_FATORES, TipoToken.PONTOVIRGULA), new() },

        { (NaoTerminal.OP_MUL, TipoToken.DIV), new() { TipoToken.DIV } },
        { (NaoTerminal.OP_MUL, TipoToken.MULT), new() { TipoToken.MULT } },
    };

    public static bool TryConsultar(NaoTerminal naoTerminal, TipoToken token, out Producao producao)
    {
        return _tabela.TryGetValue((naoTerminal, token), out producao!);
    }   
}