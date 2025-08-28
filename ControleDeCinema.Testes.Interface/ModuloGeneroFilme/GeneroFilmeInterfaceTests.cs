using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloAutenticacao;
using Docker.DotNet.Models;
using OpenQA.Selenium;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

[TestClass]
[TestCategory("Testes de Interface de GeneroFilme")]
public sealed class GeneroFilmeInterfaceTests : TestFixture
{
    [TestInitialize]
    public override void InicializarTeste()
    {
        base.InicializarTeste();

        RegistrarContaEmpresarial();
    }

    [TestMethod]
    public void Deve_Cadastrar_GeneroFilme_Corretamente()
    {
        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
             .IrPara(enderecoBase!);

            generoFilmeIndex
                .ClickCadastrar()
                .PreencherNome("Ação")
                .Confirmar();

        // Assert   
        Assert.IsTrue(generoFilmeIndex.ContemGeneroFilme("Ação"));
    }

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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
        Assert.IsTrue(generoFilmeIndex.ChamouExcecaoDeDescricao());
    }

    [TestMethod]
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

        // Assert
        Assert.IsTrue(generoFilmeIndex.ChamouAlert());
    }
}
