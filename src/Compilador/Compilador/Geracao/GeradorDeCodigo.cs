class GeradorDeCodigo
{
    private readonly List<Instrucao> _instrucoes = new();

    // indice onde a prox instrução vai cair, é o endereço usado
    // pelos saltos (DSVI/DSVF), como um label
    public int EnderecoAtual => _instrucoes.Count;

    public void Emitir(Operacao opcode, double? operando = null)
        => _instrucoes.Add(new Instrucao(opcode, operando));

    // usado pro backpatching: troca o operando de uma instrução ja emitida
    // (e.g. um DSVI/DSVF que o destino só ficou definido depois)
    public void CorrigirOperando(int endereco, double novoOperando)
        => _instrucoes[endereco] = new Instrucao(_instrucoes[endereco].Opcode, novoOperando);

    public List<Instrucao> ObterPrograma() => _instrucoes;
}