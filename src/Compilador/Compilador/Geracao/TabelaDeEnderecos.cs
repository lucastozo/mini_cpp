class TabelaDeEnderecos
{
    private readonly Dictionary<string, int> _enderecos = new();
    private int _proximoEndereco = 0;

    public int Alocar(string nome)
    {
        int endereco = _proximoEndereco++;
        _enderecos[nome] = endereco;
        return endereco;
    }

    public int ObterEndereco(string nome) 
    {
        return _enderecos[nome];
    }
}