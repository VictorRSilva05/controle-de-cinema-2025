using ControleDeCinema.Testes.Interface.Compartilhado;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

[TestClass]
[TestCategory("Testes de Interface de GeneroFilme")]
public sealed class GeneroFilmeInterfaceTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_GeneroFilme_Corretamente()
    {
        // Arrange
        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
             .IrPara(enderecoBase!);

        // Act
        generoFilmeIndex
            .ClickCadastrar()
            .PreencherNome("Ação")
            .Confirmar();

        // Assert   
        Assert.IsTrue(generoFilmeIndex.ContemGeneroFilme("Ação"));
    }

    public void Deve_Editar_GeneroFilme_Corretamente()
    {
        // Arrange
        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        generoFilmeIndex
            .ClickCadastrar()
            .PreencherNome("Ação")
            .Confirmar();

        // Act
        generoFilmeIndex
            .ClickEditar()
            .PreencherNome("Terror")
            .Confirmar();

        // Assert   
        Assert.IsTrue(generoFilmeIndex.ContemGeneroFilme("Terror"));
    }

    public void Deve_Excluir_GeneroFilme_Corretamente()
    {
        // Arrange
        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
           .IrPara(enderecoBase!);

        generoFilmeIndex
            .ClickCadastrar()
            .PreencherNome("Ação")
            .Confirmar();

        // Act
        generoFilmeIndex
            .ClickExcluir()
            .Confirmar();

        // Assert
        Assert.IsFalse(generoFilmeIndex.ContemGeneroFilme("Ação"));
    }

    public void Deve_Listar_Generos_Corretamente()
    {
        // Arrange
        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
           .IrPara(enderecoBase!);

        // Act
        generoFilmeIndex
            .ClickCadastrar()
            .PreencherNome("Ação")
            .Confirmar();

        generoFilmeIndex
            .ClickCadastrar()
            .PreencherNome("Medo")
            .Confirmar();

        // Assert 
        Assert.IsTrue(generoFilmeIndex.ContemGeneroFilme("Ação") && generoFilmeIndex.ContemGeneroFilme("Medo"));
    }

    public void Deve_Ocorrer_Erro_Ao_Deixar_Campo_Vazio()
    {
        // Arrange
        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
           .IrPara(enderecoBase!);

        // Act
        generoFilmeIndex
            .ClickCadastrar()
            .Confirmar();

        // Assert
    }

    public void Deve_Ocorrer_Erro_Ao_Tentar_Cadastrar_Descricao_Duplicada()
    {
        // Arrange
        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
           .IrPara(enderecoBase!);

        // Act
        generoFilmeIndex
            .ClickCadastrar()
            .PreencherNome("Ação")
            .Confirmar();

        generoFilmeIndex
            .ClickCadastrar()
            .PreencherNome("Ação")
            .Confirmar();
    }
}
