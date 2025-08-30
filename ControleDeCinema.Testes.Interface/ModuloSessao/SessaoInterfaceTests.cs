using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloFilme;
using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using ControleDeCinema.Testes.Interface.ModuloSala;

namespace ControleDeCinema.Testes.Interface.ModuloSessao;

/*
[TestClass]
[TestCategory("Testes de Interface de Sessão")]
public sealed class SessaoInterfaceTests : TestFixtureLocal
{   
    [TestInitialize]
    public override void InicializarTeste()
    {
        //base.InicializarTeste();

        RegistrarContaEmpresarial();

        var salaIndex = new SalaIndexPageObject(driver!)
           .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumero("1")
            .PreencherCapacidade("100")
            .Confirmar();

        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
           .IrPara(enderecoBase!);

        generoFilmeIndex
            .ClickCadastrar()
            .PreencherNome("Ação")
            .Confirmar();

        var filmeIndex = new FilmeIndexPageObject(driver!)
           .IrPara(enderecoBase!);

        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("Heat")
            .PreencherDuracao("180")
            .SelecionarGenero("Ação")
            .Confirmar();
    }

    [TestMethod]
    public void Deve_Cadastrar_Sessao_Corretamente()
    {
        // Arrange
        var sessaoIndex = new SessaoIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        // Act
        sessaoIndex
            .ClickCadastrar()
            .PreecherInicio("30/08/2025 22:00")
            .PreencherIngressos("75")
            .SelecionarFilme("Heat")
            .SelecionarSala("1")
            .Confirmar();

        // Assert
        Assert.IsTrue(sessaoIndex.ContemSessao("Heat"));
    }

    [TestMethod]
    public void Deve_Editar_Sessao_Corretamente()
    {
        // Arrange
        var sessaoIndex = new SessaoIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        // Act
        sessaoIndex
            .ClickCadastrar()
            .PreecherInicio("30/08/2025 22:00")
            .PreencherIngressos("75")
            .SelecionarFilme("Heat")
            .SelecionarSala("1")
            .Confirmar();

        sessaoIndex
            .IrPara(enderecoBase!)
            .ClickEditar()
            .PreecherInicio("01/09/2025 19:00")
            .PreencherIngressos("90")
            .SelecionarFilme("Heat")
            .SelecionarSala("1")
            .Confirmar();

        // Assert
        Assert.IsTrue(sessaoIndex.ContemSessao("Heat"));
    }
}
*/