using System.Text;

/// <summary>Imprime a AST em árvore, no mesmo estilo da ImpressoraDerivacao, para comparação.</summary>
class ImpressoraAst : IVisitorAst
{
    private readonly StringBuilder _sb = new();
    private string _prefixoAtual = "";

    public static string Gerar(ProgramaNo programa)
    {
        ImpressoraAst impressora = new();
        impressora._sb.AppendLine("PROGRAMA");
        impressora.EscreverFilhos(programa.Comandos.Cast<NoAst>().ToList());
        return impressora._sb.ToString();
    }

    // Escreve uma lista de nós-filhos, cuidando dos conectores (├──, └──, │).
    private void EscreverFilhos(List<NoAst> filhos)
    {
        string prefixoAnterior = _prefixoAtual;
        for (int i = 0; i < filhos.Count; i++)
        {
            bool ehUltimo = i == filhos.Count - 1;
            _sb.Append(prefixoAnterior);
            _sb.Append(ehUltimo ? "└── " : "├── ");
            _prefixoAtual = prefixoAnterior + (ehUltimo ? "    " : "│   ");
            filhos[i].Aceitar(this); // cada Visitar escreve seu rótulo e recursa nos próprios filhos
        }
        _prefixoAtual = prefixoAnterior;
    }

    // Escreve um "grupo" nomeado (ex: "Entao", "Senao") como se fosse um nó,
    // e dentro dele os filhos reais. Usado por SeNo/EnquantoNo.
    private void EscreverGrupos(List<(string Rotulo, List<NoAst> Filhos)> grupos)
    {
        string prefixoAnterior = _prefixoAtual;
        for (int i = 0; i < grupos.Count; i++)
        {
            bool ehUltimo = i == grupos.Count - 1;
            _sb.Append(prefixoAnterior);
            _sb.Append(ehUltimo ? "└── " : "├── ");
            _sb.AppendLine(grupos[i].Rotulo);
            _prefixoAtual = prefixoAnterior + (ehUltimo ? "    " : "│   ");
            EscreverFilhos(grupos[i].Filhos);
        }
        _prefixoAtual = prefixoAnterior;
    }

    public void Visitar(ProgramaNo no) => EscreverFilhos(no.Comandos.Cast<NoAst>().ToList());

    public void Visitar(DeclaracaoVariavelNo no)
        => _sb.AppendLine($"Declaracao ({string.Join(", ", no.Nomes)})");

    public void Visitar(AtribuicaoNo no)
    {
        _sb.AppendLine($"Atribuicao {no.NomeVariavel} =");
        EscreverFilhos(new List<NoAst> { no.Valor });
    }

    public void Visitar(EscritaNo no) => _sb.AppendLine($"Escrita {no.NomeVariavel}");

    public void Visitar(SeNo no)
    {
        _sb.AppendLine("Se");
        List<(string, List<NoAst>)> grupos = new()
        {
            ("Condicao", new List<NoAst> { no.Condicao }),
            ("Entao", no.Entao.Cast<NoAst>().ToList())
        };
        if (no.Senao != null)
            grupos.Add(("Senao", no.Senao.Cast<NoAst>().ToList()));

        EscreverGrupos(grupos);
    }

    public void Visitar(EnquantoNo no)
    {
        _sb.AppendLine("Enquanto");
        EscreverGrupos(new List<(string, List<NoAst>)>
        {
            ("Condicao", new List<NoAst> { no.Condicao }),
            ("Corpo", no.Corpo.Cast<NoAst>().ToList())
        });
    }

    public void Visitar(CondicaoNo no)
    {
        _sb.AppendLine(no.Operador.ToString());
        EscreverFilhos(new List<NoAst> { no.Esquerda, no.Direita });
    }

    public void Visitar(LiteralNo no) => _sb.AppendLine(no.Valor.ToString(System.Globalization.CultureInfo.InvariantCulture));

    public void Visitar(IdentificadorNo no) => _sb.AppendLine(no.Nome);

    public void Visitar(UnariaNo no)
    {
        _sb.AppendLine(no.Operador.ToString());
        EscreverFilhos(new List<NoAst> { no.Operando });
    }

    public void Visitar(BinariaNo no)
    {
        _sb.AppendLine(no.Operador.ToString());
        EscreverFilhos(new List<NoAst> { no.Esquerda, no.Direita });
    }

    public void Visitar(LeituraNo no) => _sb.AppendLine("LerDouble");
}