static class TabelaM
{
    private static readonly Dictionary<(NaoTerminal, TipoToken), Producao> _tabela = new()
    {
        // PROG -> CERQUILHA INCLUDE ME IOSTREAM MA USING NAMESPACE STD PONTOVIRGULA INT MAIN ESQPAREN DIRPAREN ESQCHAVE CMDS DIRCHAVE
        { (NaoTerminal.PROG, TipoToken.CERQUILHA), new() {
            new Acao(p => p.Gerador.Emitir(Operacao.INPP)),
            TipoToken.CERQUILHA, TipoToken.INCLUDE, TipoToken.ME, TipoToken.IOSTREAM, TipoToken.MA,
            TipoToken.USING, TipoToken.NAMESPACE, TipoToken.STD, TipoToken.PONTOVIRGULA,
            TipoToken.INT, TipoToken.MAIN, TipoToken.ESQPAREN, TipoToken.DIRPAREN,
            TipoToken.ESQCHAVE, NaoTerminal.CMDS, TipoToken.DIRCHAVE,
            new Acao(p => p.Gerador.Emitir(Operacao.PARA))
        } },

        // DC -> VAR MAIS_CMDS
        { (NaoTerminal.DC, TipoToken.DOUBLE), new() { NaoTerminal.VAR, NaoTerminal.MAIS_CMDS } },

        // VAR -> TIPO VARS
        { (NaoTerminal.VAR, TipoToken.DOUBLE), new() { NaoTerminal.TIPO, NaoTerminal.VARS } },

        // VARS -> ID MAIS_VAR [DECLARAÇÃO, registra na tabela de símbolos, aloca 1 pos]
        { (NaoTerminal.VARS, TipoToken.ID), new() {
            new Acao(p => {
                string nome = p.TokenAtual.Lexema;
                p.TabelaDeSimbolos.Declarar(nome);
                p.TabelaDeEnderecos.Alocar(nome);
                p.Gerador.Emitir(Operacao.ALME, 1);
            }),
            TipoToken.ID, NaoTerminal.MAIS_VAR
        } },

        // MAIS_VAR -> VIRGULA VARS | ε
        { (NaoTerminal.MAIS_VAR, TipoToken.VIRGULA), new() { TipoToken.VIRGULA, NaoTerminal.VARS } },
        { (NaoTerminal.MAIS_VAR, TipoToken.PONTOVIRGULA), new() }, // ε

        // TIPO -> DOUBLE
        { (NaoTerminal.TIPO, TipoToken.DOUBLE), new() { TipoToken.DOUBLE } },

        // CMDS -> CMD MAIS_CMDS | CMD_COND CMDS | DC | ε
        { (NaoTerminal.CMDS, TipoToken.COUT), new() { NaoTerminal.CMD, NaoTerminal.MAIS_CMDS } },
        { (NaoTerminal.CMDS, TipoToken.ID), new() { NaoTerminal.CMD, NaoTerminal.MAIS_CMDS } },
        { (NaoTerminal.CMDS, TipoToken.IF), new() { NaoTerminal.CMD_COND, NaoTerminal.CMDS } },
        { (NaoTerminal.CMDS, TipoToken.WHILE), new() { NaoTerminal.CMD_COND, NaoTerminal.CMDS } },
        { (NaoTerminal.CMDS, TipoToken.DOUBLE), new() { NaoTerminal.DC } },
        { (NaoTerminal.CMDS, TipoToken.DIRCHAVE), new() }, // ε

        // MAIS_CMDS -> PONTOVIRGULA CMDS
        { (NaoTerminal.MAIS_CMDS, TipoToken.PONTOVIRGULA), new() { TipoToken.PONTOVIRGULA, NaoTerminal.CMDS } },

        // CMD_COND -> IF ( CONDICAO ) { CMDS } PFALSA
        { (NaoTerminal.CMD_COND, TipoToken.IF), new() {
            TipoToken.IF, TipoToken.ESQPAREN, NaoTerminal.CONDICAO, TipoToken.DIRPAREN,
            new Acao(p => {
                p.Gerador.Emitir(Operacao.DSVF, null); // destino ainda desconhecido
                p.PendentesPuloElse.Push(p.Gerador.EnderecoAtual - 1);
            }),
            TipoToken.ESQCHAVE, NaoTerminal.CMDS, TipoToken.DIRCHAVE,
            new Acao(p => {
                p.Gerador.Emitir(Operacao.DSVI, null); // pula o PFALSA inteiro
                p.PendentesPuloFim.Push(p.Gerador.EnderecoAtual - 1);

                int enderecoDsvf = p.PendentesPuloElse.Pop();
                p.Gerador.CorrigirOperando(enderecoDsvf, p.Gerador.EnderecoAtual);
            }),
            NaoTerminal.PFALSA,
            new Acao(p => {
                int enderecoDsvi = p.PendentesPuloFim.Pop();
                p.Gerador.CorrigirOperando(enderecoDsvi, p.Gerador.EnderecoAtual);
            })
        } },

        // CMD_COND -> WHILE ( CONDICAO ) { CMDS }
        // Backpatching: guarda o endereço de início (pra voltar), DSVF pula o corpo se falso.
        { (NaoTerminal.CMD_COND, TipoToken.WHILE), new() {
            new Acao(p => p.PilhaInicioLaco.Push(p.Gerador.EnderecoAtual)),
            TipoToken.WHILE, TipoToken.ESQPAREN, NaoTerminal.CONDICAO, TipoToken.DIRPAREN,
            new Acao(p => {
                p.Gerador.Emitir(Operacao.DSVF, null);
                p.PendentesPuloFim.Push(p.Gerador.EnderecoAtual - 1);
            }),
            TipoToken.ESQCHAVE, NaoTerminal.CMDS, TipoToken.DIRCHAVE,
            new Acao(p => {
                int inicio = p.PilhaInicioLaco.Pop();
                p.Gerador.Emitir(Operacao.DSVI, inicio);

                int enderecoDsvf = p.PendentesPuloFim.Pop();
                p.Gerador.CorrigirOperando(enderecoDsvf, p.Gerador.EnderecoAtual);
            })
        } },

        // CMD -> COUT INSERCAO ID INSERCAO ENDL
        { (NaoTerminal.CMD, TipoToken.COUT), new() {
            TipoToken.COUT, TipoToken.INSERCAO,
            new Acao(p => {
                string nome = p.TokenAtual.Lexema;
                p.TabelaDeSimbolos.Verificar(nome);
                p.Gerador.Emitir(Operacao.CRVL, p.TabelaDeEnderecos.ObterEndereco(nome));
                p.Gerador.Emitir(Operacao.IMPR);
            }),
            TipoToken.ID, TipoToken.INSERCAO, TipoToken.ENDL
        } },

        // CMD -> ID RESTO_IDENT   [alvo de atribuição: ARMZ só depois do lado direito inteiro]
        { (NaoTerminal.CMD, TipoToken.ID), new() {
            new Acao(p => {
                string nome = p.TokenAtual.Lexema;
                p.TabelaDeSimbolos.Verificar(nome);
                p.AlvoAtribuicaoPendente = nome;
            }),
            TipoToken.ID,
            NaoTerminal.RESTO_IDENT,
            new Acao(p => {
                string nome = p.AlvoAtribuicaoPendente!;
                p.Gerador.Emitir(Operacao.ARMZ, p.TabelaDeEnderecos.ObterEndereco(nome));
                p.AlvoAtribuicaoPendente = null;
            })
        } },

        // PFALSA -> ELSE ESQCHAVE CMDS DIRCHAVE | ε
        { (NaoTerminal.PFALSA, TipoToken.ELSE), new() {
            TipoToken.ELSE, TipoToken.ESQCHAVE, NaoTerminal.CMDS, TipoToken.DIRCHAVE
        } },
        { (NaoTerminal.PFALSA, TipoToken.COUT), new() },
        { (NaoTerminal.PFALSA, TipoToken.ID), new() },
        { (NaoTerminal.PFALSA, TipoToken.IF), new() },
        { (NaoTerminal.PFALSA, TipoToken.WHILE), new() },
        { (NaoTerminal.PFALSA, TipoToken.DOUBLE), new() },
        { (NaoTerminal.PFALSA, TipoToken.DIRCHAVE), new() },

        // RESTO_IDENT -> ATRIBUICAO EXP_IDENT
        { (NaoTerminal.RESTO_IDENT, TipoToken.ATRIBUICAO), new() { TipoToken.ATRIBUICAO, NaoTerminal.EXP_IDENT } },

        // EXP_IDENT -> EXPRESSAO | LER_DOUBLE ESQPAREN DIRPAREN
        { (NaoTerminal.EXP_IDENT, TipoToken.MENOS), new() { NaoTerminal.EXPRESSAO } },
        { (NaoTerminal.EXP_IDENT, TipoToken.ID), new() { NaoTerminal.EXPRESSAO } },
        { (NaoTerminal.EXP_IDENT, TipoToken.NUMERO_REAL), new() { NaoTerminal.EXPRESSAO } },
        { (NaoTerminal.EXP_IDENT, TipoToken.ESQPAREN), new() { NaoTerminal.EXPRESSAO } },
        { (NaoTerminal.EXP_IDENT, TipoToken.LER_DOUBLE), new() {
            new Acao(p => p.Gerador.Emitir(Operacao.LEIT)),
            TipoToken.LER_DOUBLE, TipoToken.ESQPAREN, TipoToken.DIRPAREN
        } },

        // CONDICAO -> EXPRESSAO RELACAO EXPRESSAO
        { (NaoTerminal.CONDICAO, TipoToken.MENOS), new() {
            NaoTerminal.EXPRESSAO, NaoTerminal.RELACAO, NaoTerminal.EXPRESSAO,
            new Acao(p => p.Gerador.Emitir(p.RelacaoPendente!.Value))
        } },
        { (NaoTerminal.CONDICAO, TipoToken.ID), new() {
            NaoTerminal.EXPRESSAO, NaoTerminal.RELACAO, NaoTerminal.EXPRESSAO,
            new Acao(p => p.Gerador.Emitir(p.RelacaoPendente!.Value))
        } },
        { (NaoTerminal.CONDICAO, TipoToken.NUMERO_REAL), new() {
            NaoTerminal.EXPRESSAO, NaoTerminal.RELACAO, NaoTerminal.EXPRESSAO,
            new Acao(p => p.Gerador.Emitir(p.RelacaoPendente!.Value))
        } },
        { (NaoTerminal.CONDICAO, TipoToken.ESQPAREN), new() {
            NaoTerminal.EXPRESSAO, NaoTerminal.RELACAO, NaoTerminal.EXPRESSAO,
            new Acao(p => p.Gerador.Emitir(p.RelacaoPendente!.Value))
        } },

        // RELACAO -> IG | DES | MAI | MI | MA | ME
        // Cada alternativa só guarda QUAL comparação será usada; quem emite é a CONDICAO.
        { (NaoTerminal.RELACAO, TipoToken.IG), new() { new Acao(p => p.RelacaoPendente = Operacao.CPIG), TipoToken.IG } },
        { (NaoTerminal.RELACAO, TipoToken.DES), new() { new Acao(p => p.RelacaoPendente = Operacao.CDES), TipoToken.DES } },
        { (NaoTerminal.RELACAO, TipoToken.MAI), new() { new Acao(p => p.RelacaoPendente = Operacao.CMAI), TipoToken.MAI } },
        { (NaoTerminal.RELACAO, TipoToken.MI), new() { new Acao(p => p.RelacaoPendente = Operacao.CPMI), TipoToken.MI } },
        { (NaoTerminal.RELACAO, TipoToken.MA), new() { new Acao(p => p.RelacaoPendente = Operacao.CPMA), TipoToken.MA } },
        { (NaoTerminal.RELACAO, TipoToken.ME), new() { new Acao(p => p.RelacaoPendente = Operacao.CPME), TipoToken.ME } },

        // EXPRESSAO -> TERMO OUTROS_TERMOS
        { (NaoTerminal.EXPRESSAO, TipoToken.MENOS), new() { NaoTerminal.TERMO, NaoTerminal.OUTROS_TERMOS } },
        { (NaoTerminal.EXPRESSAO, TipoToken.ID), new() { NaoTerminal.TERMO, NaoTerminal.OUTROS_TERMOS } },
        { (NaoTerminal.EXPRESSAO, TipoToken.NUMERO_REAL), new() { NaoTerminal.TERMO, NaoTerminal.OUTROS_TERMOS } },
        { (NaoTerminal.EXPRESSAO, TipoToken.ESQPAREN), new() { NaoTerminal.TERMO, NaoTerminal.OUTROS_TERMOS } },

        // TERMO -> OP_UN FATOR MAIS_FATORES
        // INVE só emitido ao final, e só se o OP_UN desta ocorrência foi MENOS.
        { (NaoTerminal.TERMO, TipoToken.MENOS), new() {
            NaoTerminal.OP_UN, NaoTerminal.FATOR, NaoTerminal.MAIS_FATORES,
            new Acao(p => { if (p.PilhaUnario.Pop()) p.Gerador.Emitir(Operacao.INVE); })
        } },
        { (NaoTerminal.TERMO, TipoToken.ID), new() {
            NaoTerminal.OP_UN, NaoTerminal.FATOR, NaoTerminal.MAIS_FATORES,
            new Acao(p => { if (p.PilhaUnario.Pop()) p.Gerador.Emitir(Operacao.INVE); })
        } },
        { (NaoTerminal.TERMO, TipoToken.NUMERO_REAL), new() {
            NaoTerminal.OP_UN, NaoTerminal.FATOR, NaoTerminal.MAIS_FATORES,
            new Acao(p => { if (p.PilhaUnario.Pop()) p.Gerador.Emitir(Operacao.INVE); })
        } },
        { (NaoTerminal.TERMO, TipoToken.ESQPAREN), new() {
            NaoTerminal.OP_UN, NaoTerminal.FATOR, NaoTerminal.MAIS_FATORES,
            new Acao(p => { if (p.PilhaUnario.Pop()) p.Gerador.Emitir(Operacao.INVE); })
        } },

        // OP_UN -> MENOS | ε
        // Empilha se ESTE OP_UN pediu inversão de sinal (consumido pelo TERMO acima).
        { (NaoTerminal.OP_UN, TipoToken.MENOS), new() { new Acao(p => p.PilhaUnario.Push(true)), TipoToken.MENOS } },
        { (NaoTerminal.OP_UN, TipoToken.ID), new() { new Acao(p => p.PilhaUnario.Push(false)) } },
        { (NaoTerminal.OP_UN, TipoToken.NUMERO_REAL), new() { new Acao(p => p.PilhaUnario.Push(false)) } },
        { (NaoTerminal.OP_UN, TipoToken.ESQPAREN), new() { new Acao(p => p.PilhaUnario.Push(false)) } },

        // FATOR -> ID | NUMERO_REAL | ( EXPRESSAO )
        { (NaoTerminal.FATOR, TipoToken.ID), new() {
            new Acao(p => {
                string nome = p.TokenAtual.Lexema;
                p.TabelaDeSimbolos.Verificar(nome);
                p.Gerador.Emitir(Operacao.CRVL, p.TabelaDeEnderecos.ObterEndereco(nome));
            }),
            TipoToken.ID
        } },
        { (NaoTerminal.FATOR, TipoToken.NUMERO_REAL), new() {
            new Acao(p => p.Gerador.Emitir(Operacao.CRCT, double.Parse(p.TokenAtual.Lexema, System.Globalization.CultureInfo.InvariantCulture))),
            TipoToken.NUMERO_REAL
        } },
        { (NaoTerminal.FATOR, TipoToken.ESQPAREN), new() { TipoToken.ESQPAREN, NaoTerminal.EXPRESSAO, TipoToken.DIRPAREN } },

        // OUTROS_TERMOS -> OP_AD TERMO OUTROS_TERMOS | ε
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.MAIS), new() {
            NaoTerminal.OP_AD, NaoTerminal.TERMO,
            new Acao(p => p.Gerador.Emitir(p.PilhaOperadorAditivo.Pop())),
            NaoTerminal.OUTROS_TERMOS
        } },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.MENOS), new() {
            NaoTerminal.OP_AD, NaoTerminal.TERMO,
            new Acao(p => p.Gerador.Emitir(p.PilhaOperadorAditivo.Pop())),
            NaoTerminal.OUTROS_TERMOS
        } },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.IG), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.DES), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.MAI), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.MI), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.MA), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.ME), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.DIRPAREN), new() },
        { (NaoTerminal.OUTROS_TERMOS, TipoToken.PONTOVIRGULA), new() },

        // OP_AD -> MAIS | MENOS
        // Só guarda qual operador foi visto (consumido pelo OUTROS_TERMOS acima).
        { (NaoTerminal.OP_AD, TipoToken.MAIS), new() { new Acao(p => p.PilhaOperadorAditivo.Push(Operacao.SOMA)), TipoToken.MAIS } },
        { (NaoTerminal.OP_AD, TipoToken.MENOS), new() { new Acao(p => p.PilhaOperadorAditivo.Push(Operacao.SUBT)), TipoToken.MENOS } },

        // MAIS_FATORES -> OP_MUL FATOR MAIS_FATORES | ε
        { (NaoTerminal.MAIS_FATORES, TipoToken.MULT), new() {
            NaoTerminal.OP_MUL, NaoTerminal.FATOR,
            new Acao(p => p.Gerador.Emitir(p.PilhaOperadorMultiplicativo.Pop())),
            NaoTerminal.MAIS_FATORES
        } },
        { (NaoTerminal.MAIS_FATORES, TipoToken.DIV), new() {
            NaoTerminal.OP_MUL, NaoTerminal.FATOR,
            new Acao(p => p.Gerador.Emitir(p.PilhaOperadorMultiplicativo.Pop())),
            NaoTerminal.MAIS_FATORES
        } },
        { (NaoTerminal.MAIS_FATORES, TipoToken.MAIS), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.MENOS), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.IG), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.DES), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.MAI), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.MI), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.MA), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.ME), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.DIRPAREN), new() },
        { (NaoTerminal.MAIS_FATORES, TipoToken.PONTOVIRGULA), new() },

        // OP_MUL -> MULT | DIV
        { (NaoTerminal.OP_MUL, TipoToken.MULT), new() { new Acao(p => p.PilhaOperadorMultiplicativo.Push(Operacao.MULT)), TipoToken.MULT } },
        { (NaoTerminal.OP_MUL, TipoToken.DIV), new() { new Acao(p => p.PilhaOperadorMultiplicativo.Push(Operacao.DIVI)), TipoToken.DIV } },
    };

    public static bool TryConsultar(NaoTerminal naoTerminal, TipoToken token, out Producao? producao)
        => _tabela.TryGetValue((naoTerminal, token), out producao);
}