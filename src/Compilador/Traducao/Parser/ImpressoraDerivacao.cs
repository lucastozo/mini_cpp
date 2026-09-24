/// <summary>Gera uma representação em texto da parse tree, para depuração.</summary>
static class ImpressoraDerivacao
{
    public static string Gerar(NoDerivacao raiz)
    {
        System.Text.StringBuilder sb = new();
        sb.AppendLine(Descrever(raiz));
        GerarFilhos(raiz, "", sb);
        return sb.ToString();
    }

    private static void GerarFilhos(NoDerivacao no, string prefixo, System.Text.StringBuilder sb)
    {
        for (int i = 0; i < no.Filhos.Count; i++)
        {
            NoDerivacao filho = no.Filhos[i];
            bool ehUltimo = i == no.Filhos.Count - 1;

            sb.Append(prefixo);
            sb.Append(ehUltimo ? "└── " : "├── ");
            sb.AppendLine(Descrever(filho));

            // Os descendentes deste filho herdam o prefixo, com uma barra vertical
            // se ainda existirem irmãos abaixo dele.
            string novoPrefixo = prefixo + (ehUltimo ? "    " : "│   ");
            GerarFilhos(filho, novoPrefixo, sb);
        }
    }

    private static string Descrever(NoDerivacao no)
    {
        return no.Simbolo switch
        {
            Terminal terminal => $"{terminal.Tipo} '{no.Token?.Lexema}'",
            NaoTerminalSimbolo nt when no.EhVazio => $"{nt.Tipo} (ε)",
            NaoTerminalSimbolo nt => nt.Tipo.ToString(),
            _ => throw new InvalidOperationException("Símbolo desconhecido.")
        };
    }
}