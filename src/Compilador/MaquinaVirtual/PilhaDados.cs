class PilhaDados
{
    private readonly List<double> dados = new();
    public void Push(double valor) => dados.Add(valor);

    public double Pop()
    {
        double topo = dados[^1];
        dados.RemoveAt(dados.Count - 1);
        return topo;
    }
    public void Clear() => dados.Clear();

    public double this[int indice]
    {
        get => dados[indice];
        set => dados[indice] = value;
    }
}