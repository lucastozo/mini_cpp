class MontadorDeInstrucoes
{
    public static List<Instrucao> Montar(string[] codigoObjeto)
    {
        var programa = new List<Instrucao>();

        foreach (string linha in codigoObjeto)
        {
            string[] partes = linha.Split(' ');

            if (!Enum.TryParse(partes[0], out Operacao operacao))
                throw new Exception($"Operação {partes[0]} não existe.");

            double? operando = partes.Length == 2
                ? double.Parse(partes[1], System.Globalization.CultureInfo.InvariantCulture)
                : null;

            programa.Add(new Instrucao(operacao, operando));
        }

        return programa;
    }
}