class TabelaDeSimbolos
{
    private readonly HashSet<string> _declaradas = new();

    public void Declarar(string nome)
    {
        if (!_declaradas.Add(nome))
            throw new Exception($"Variável '{nome}' já foi declarada.");
    }

    public void Verificar(string nome)
    {
        if (!_declaradas.Contains(nome))
            throw new Exception($"Variável '{nome}' usada sem ter sido declarada.");
    }
}