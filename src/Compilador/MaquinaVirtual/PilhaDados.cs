class PilhaDados
{
    private readonly List<double> _dados = new();
    public void Push(double valor) => _dados.Add(valor);

    public double Pop()
    {
        double topo = _dados[^1];
        _dados.RemoveAt(_dados.Count - 1);
        return topo;
    }
    public void Clear() => _dados.Clear();

    public double this[int indice]
    {
        get => _dados[indice];
        set => _dados[indice] = value;
    }
}