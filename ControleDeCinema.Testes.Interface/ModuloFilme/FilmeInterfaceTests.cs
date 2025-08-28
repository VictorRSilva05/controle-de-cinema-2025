using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

namespace ControleDeCinema.Testes.Interface.ModuloFilme;

[TestClass]
[TestCategory("Testes de Interface de Filme")]
public sealed class FilmeInterfaceTests : TestFixture
{
    [TestInitialize]
    public override void InicializarTeste()
    {
        base.InicializarTeste();

        RegistrarContaEmpresarial();

        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        generoFilmeIndex
            .ClickCadastrar()
            .PreencherNome("Ação")
            .Confirmar();

        generoFilmeIndex
            .ClickCadastrar()
            .PreencherNome("Medo")
            .Confirmar();
    }

    [TestMethod]
    public void Deve_Cadastrar_Filme_Corretamente()
    {
        // Arrange
        var filmeIndex = new FilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        // Act
        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("Heat")
            .PreencherDuracao("180")
            .SelecionarGenero("Ação")
            .Confirmar();

        // Assert
        Assert.IsTrue(filmeIndex.ContemFilme("Heat"));
    }

    [TestMethod]
    public void Deve_Editar_Titulo_Corretamente()
    {
        // Arrange
        var filmeIndex = new FilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("Heat")
            .PreencherDuracao("180")
            .SelecionarGenero("Ação")
            .Confirmar();

        // Act
        filmeIndex
            .ClickEditar()
            .PreencherTitulo("The Audition")
            .PreencherDuracao("120")
            .SelecionarGenero("Medo")
            .Confirmar();

        // Assert
        Assert.IsTrue(filmeIndex.ContemFilme("The Audition"));
    }

    [TestMethod]
    public void Deve_Excluir_Titulo_Corretamente()
    {
        // Arrange
        var filmeIndex = new FilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("Heat")
            .PreencherDuracao("180")
            .SelecionarGenero("Ação")
            .Confirmar();

        // Act
        filmeIndex
            .ClickExcluir()
            .Confirmar();    

        // Assert
        Assert.IsFalse(filmeIndex.ContemFilme("Heat"));
    }

    [TestMethod]
    public void Deve_Listar_Filmes_Corretamente()
    {
        // Arrange
        var filmeIndex = new FilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("Heat")
            .PreencherDuracao("180")
            .SelecionarGenero("Ação")
            .Confirmar();

        // Act
        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("The Audition")
            .PreencherDuracao("120")
            .SelecionarGenero("Medo")
            .Confirmar();

        // Assert
        Assert.IsTrue(filmeIndex.ContemFilme("The Audition") && filmeIndex.ContemFilme("Heat"));
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Ao_Deixar_Campo_Titulo_Vazio()
    {
        // Arrange
        var filmeIndex = new FilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        // Act
        filmeIndex
            .ClickCadastrar()
            .PreencherDuracao("180")
            .SelecionarGenero("Ação")
            .Confirmar();

        // Assert
        Assert.IsTrue(filmeIndex.ChamouExcecaoDeTitulo());
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Ao_Deixar_Campo_Duracao_Vazio()
    {
        // Arrange
        var filmeIndex = new FilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        // Act
        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("Heat")
            .SelecionarGenero("Ação")
            .Confirmar();

        // Assert
        Assert.IsTrue(filmeIndex.ChamouExcecaoDeDuracao());
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Ao_Deixar_Campo_Genero_Vazio()
    {
        // Arrange
        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!); 

        generoFilmeIndex
            .ClickExcluir()
            .Confirmar();

        generoFilmeIndex
            .ClickExcluir()
            .Confirmar();

        var filmeIndex = new FilmeIndexPageObject (driver!)
            .IrPara(enderecoBase!);

        // Act
        filmeIndex
          .ClickCadastrar()
          .PreencherTitulo("The Audition")
          .PreencherDuracao("120")
          .Confirmar();

        // Assert
        Assert.IsTrue(filmeIndex.ChamouExcecaoDeGenero());
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Caso_A_Duracao_Seja_Negativa()
    {
        // Arrange
        var filmeIndex = new FilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        // Act
        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("Heat")
            .PreencherDuracao("-180")
            .SelecionarGenero("Ação")
            .Confirmar();

        // Assert
        Assert.IsTrue(filmeIndex.ChamouExcecaoDeDuracao());
    }

    [TestMethod]
    public void Deve_Ocorer_Erro_Caso_O_Mesmo_Titulo_Seja_Cadastrado()
    {
        // Arrange
        var filmeIndex = new FilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("Heat")
            .PreencherDuracao("180")
            .SelecionarGenero("Ação")
            .Confirmar();

        // Act
        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("Heat")
            .PreencherDuracao("180")
            .SelecionarGenero("Ação")
            .Confirmar();

        // Assert
        Assert.IsTrue(filmeIndex.ChamouAlert());
    }
}
