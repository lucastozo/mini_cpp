using System.Globalization;

/// <summary>
/// Traduz a árvore de derivação (CST), já validada pelo Parser, para a AST.
/// Cada método corresponde a um não-terminal da gramática.
/// </summary>
class ConstrutorAst
{
    public ProgramaNo Construir(NoDerivacao prog)
    {
        NoDerivacao cmds = prog.Filho(NaoTerminal.CMDS);
        List<ComandoNo> comandos = ConstruirCmds(cmds);
        return new ProgramaNo(comandos);
    }

    // CMDS -> CMD MAIS_CMDS | CMD_COND CMDS | DC | ε
    // Percorre a cadeia de CMDS até encontrar ε, achatando tudo numa lista só.
    private List<ComandoNo> ConstruirCmds(NoDerivacao cmds)
    {
        List<ComandoNo> lista = new();
        NoDerivacao atual = cmds;

        while (!atual.EhVazio)
        {
            NaoTerminal primeiraProducao = ((NaoTerminalSimbolo)atual.Filhos[0].Simbolo).Tipo;

            switch (primeiraProducao)
            {
                case NaoTerminal.CMD:
                    lista.Add(ConstruirCmd(atual.Filhos[0]));
                    // MAIS_CMDS -> PONTOVIRGULA CMDS
                    atual = atual.Filhos[1].Filhos[1];
                    break;

                case NaoTerminal.CMD_COND:
                    lista.Add(ConstruirCmdCond(atual.Filhos[0]));
                    // CMD_COND CMDS -> o segundo filho já é o próximo CMDS
                    atual = atual.Filhos[1];
                    break;

                case NaoTerminal.DC:
                    NoDerivacao dc = atual.Filhos[0];
                    lista.Add(ConstruirDc(dc));
                    // DC -> VAR MAIS_CMDS, MAIS_CMDS -> PONTOVIRGULA CMDS
                    atual = dc.Filhos[1].Filhos[1];
                    break;

                default:
                    throw new InvalidOperationException($"Produção inesperada de CMDS: '{primeiraProducao}'.");
            }
        }

        return lista;
    }

    // DC -> VAR MAIS_CMDS   |   VAR -> TIPO VARS
    private DeclaracaoVariavelNo ConstruirDc(NoDerivacao dc)
    {
        NoDerivacao var = dc.Filhos[0];
        NoDerivacao vars = var.Filho(NaoTerminal.VARS);
        List<string> nomes = ConstruirVars(vars);
        return new DeclaracaoVariavelNo(nomes);
    }

    // VARS -> ID MAIS_VAR   |   MAIS_VAR -> VIRGULA VARS | ε
    private List<string> ConstruirVars(NoDerivacao vars)
    {
        List<string> nomes = new();
        NoDerivacao atual = vars;

        while (true)
        {
            nomes.Add(atual.FilhoToken(TipoToken.ID).Lexema);

            NoDerivacao maisVar = atual.Filho(NaoTerminal.MAIS_VAR);
            if (maisVar.EhVazio)
                break;

            atual = maisVar.Filho(NaoTerminal.VARS);
        }

        return nomes;
    }

    // CMD -> COUT INSERCAO ID INSERCAO ENDL   |   ID RESTO_IDENT
    private ComandoNo ConstruirCmd(NoDerivacao cmd)
    {
        bool ehEscrita = cmd.Filhos[0].Simbolo is Terminal t && t.Tipo == TipoToken.COUT;

        if (ehEscrita)
        {
            string nomeVariavel = cmd.FilhoToken(TipoToken.ID).Lexema;
            return new EscritaNo(nomeVariavel);
        }
        else
        {
            string nomeVariavel = cmd.FilhoToken(TipoToken.ID).Lexema;
            NoDerivacao restoIdent = cmd.Filho(NaoTerminal.RESTO_IDENT);
            NoDerivacao expIdent = restoIdent.Filho(NaoTerminal.EXP_IDENT);
            ExpressaoNo valor = ConstruirExpIdent(expIdent);
            return new AtribuicaoNo(nomeVariavel, valor);
        }
    }

    // EXP_IDENT -> EXPRESSAO | LER_DOUBLE ESQPAREN DIRPAREN
    private ExpressaoNo ConstruirExpIdent(NoDerivacao expIdent)
    {
        bool ehLeitura = expIdent.Filhos[0].Simbolo is Terminal t && t.Tipo == TipoToken.LER_DOUBLE;
        return ehLeitura
            ? new LeituraNo()
            : ConstruirExpressao(expIdent.Filho(NaoTerminal.EXPRESSAO));
    }

    // CMD_COND -> IF ESQPAREN CONDICAO DIRPAREN ESQCHAVE CMDS DIRCHAVE PFALSA
    //           | WHILE ESQPAREN CONDICAO DIRPAREN ESQCHAVE CMDS DIRCHAVE
    private ComandoNo ConstruirCmdCond(NoDerivacao cmdCond)
    {
        bool ehSe = cmdCond.Filhos[0].Simbolo is Terminal t && t.Tipo == TipoToken.IF;

        CondicaoNo condicao = ConstruirCondicao(cmdCond.Filho(NaoTerminal.CONDICAO));
        List<ComandoNo> corpo = ConstruirCmds(cmdCond.Filho(NaoTerminal.CMDS));

        if (ehSe)
        {
            List<ComandoNo>? senao = ConstruirPfalsa(cmdCond.Filho(NaoTerminal.PFALSA));
            return new SeNo(condicao, corpo, senao);
        }
        else
        {
            return new EnquantoNo(condicao, corpo);
        }
    }

    // PFALSA -> ELSE ESQCHAVE CMDS DIRCHAVE | ε
    private List<ComandoNo>? ConstruirPfalsa(NoDerivacao pfalsa)
    {
        if (pfalsa.EhVazio)
            return null;

        return ConstruirCmds(pfalsa.Filho(NaoTerminal.CMDS));
    }

    // CONDICAO -> EXPRESSAO RELACAO EXPRESSAO
    private CondicaoNo ConstruirCondicao(NoDerivacao condicao)
    {
        ExpressaoNo esquerda = ConstruirExpressao(condicao.Filhos[0]);
        OperadorRelacional operador = ConstruirRelacao(condicao.Filhos[1]);
        ExpressaoNo direita = ConstruirExpressao(condicao.Filhos[2]);
        return new CondicaoNo(esquerda, operador, direita);
    }

    // RELACAO -> IG | DES | MAI | MI | MA | ME
    private OperadorRelacional ConstruirRelacao(NoDerivacao relacao)
    {
        Terminal terminal = (Terminal)relacao.Filhos[0].Simbolo;
        return terminal.Tipo switch
        {
            TipoToken.IG => OperadorRelacional.Igual,
            TipoToken.DES => OperadorRelacional.Diferente,
            TipoToken.MAI => OperadorRelacional.MaiorIgual,
            TipoToken.MI => OperadorRelacional.MenorIgual,
            TipoToken.MA => OperadorRelacional.Maior,
            TipoToken.ME => OperadorRelacional.Menor,
            _ => throw new InvalidOperationException($"Operador relacional inesperado: '{terminal.Tipo}'.")
        };
    }

    // EXPRESSAO -> TERMO OUTROS_TERMOS
    // Fold à esquerda: (((termo1 op termo2) op termo3) ...)
    private ExpressaoNo ConstruirExpressao(NoDerivacao expressao)
    {
        ExpressaoNo esquerda = ConstruirTermo(expressao.Filhos[0]);
        NoDerivacao outrosTermos = expressao.Filhos[1];

        while (!outrosTermos.EhVazio)
        {
            OperadorBinario operador = ConstruirOpAd(outrosTermos.Filhos[0]);
            ExpressaoNo direita = ConstruirTermo(outrosTermos.Filhos[1]);
            esquerda = new BinariaNo(operador, esquerda, direita);
            outrosTermos = outrosTermos.Filhos[2];
        }

        return esquerda;
    }

    // OP_AD -> MAIS | MENOS
    private OperadorBinario ConstruirOpAd(NoDerivacao opAd)
    {
        Terminal terminal = (Terminal)opAd.Filhos[0].Simbolo;
        return terminal.Tipo switch
        {
            TipoToken.MAIS => OperadorBinario.Soma,
            TipoToken.MENOS => OperadorBinario.Subtracao,
            _ => throw new InvalidOperationException($"Operador aditivo inesperado: '{terminal.Tipo}'.")
        };
    }

    // TERMO -> OP_UN FATOR MAIS_FATORES
    private ExpressaoNo ConstruirTermo(NoDerivacao termo)
    {
        ExpressaoNo fator = ConstruirFator(termo.Filhos[1]);

        // OP_UN -> MENOS | ε — se presente, aplica só ao primeiro fator.
        NoDerivacao opUn = termo.Filhos[0];
        if (!opUn.EhVazio)
            fator = new UnariaNo(OperadorUnario.Negativo, fator);

        NoDerivacao maisFatores = termo.Filhos[2];
        while (!maisFatores.EhVazio)
        {
            OperadorBinario operador = ConstruirOpMul(maisFatores.Filhos[0]);
            ExpressaoNo proximoFator = ConstruirFator(maisFatores.Filhos[1]);
            fator = new BinariaNo(operador, fator, proximoFator);
            maisFatores = maisFatores.Filhos[2];
        }

        return fator;
    }

    // OP_MUL -> MULT | DIV
    private OperadorBinario ConstruirOpMul(NoDerivacao opMul)
    {
        Terminal terminal = (Terminal)opMul.Filhos[0].Simbolo;
        return terminal.Tipo switch
        {
            TipoToken.MULT => OperadorBinario.Multiplicacao,
            TipoToken.DIV => OperadorBinario.Divisao,
            _ => throw new InvalidOperationException($"Operador multiplicativo inesperado: '{terminal.Tipo}'.")
        };
    }

    // FATOR -> ID | NUMERO_REAL | ESQPAREN EXPRESSAO DIRPAREN
    private ExpressaoNo ConstruirFator(NoDerivacao fator)
    {
        Simbolo primeiro = fator.Filhos[0].Simbolo;

        if (primeiro is Terminal t && t.Tipo == TipoToken.ID)
            return new IdentificadorNo(fator.Filhos[0].Token!.Lexema);

        if (primeiro is Terminal t2 && t2.Tipo == TipoToken.NUMERO_REAL)
            return new LiteralNo(double.Parse(fator.Filhos[0].Token!.Lexema, CultureInfo.InvariantCulture));

        // ESQPAREN EXPRESSAO DIRPAREN — os parênteses não viram nó, só delimitam.
        return ConstruirExpressao(fator.Filho(NaoTerminal.EXPRESSAO));
    }
}