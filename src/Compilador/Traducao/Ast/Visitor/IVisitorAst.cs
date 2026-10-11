interface IVisitorAst
{
    void Visitar(ProgramaNo no);
    void Visitar(DeclaracaoVariavelNo no);
    void Visitar(AtribuicaoNo no);
    void Visitar(EscritaNo no);
    void Visitar(SeNo no);
    void Visitar(EnquantoNo no);
    void Visitar(CondicaoNo no);
    void Visitar(LiteralNo no);
    void Visitar(IdentificadorNo no);
    void Visitar(UnariaNo no);
    void Visitar(BinariaNo no);
    void Visitar(LeituraNo no);
}