using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloSessao;

namespace ControleDeCinema.Testes.Unidade.ModuloSessao;

[TestClass]
[TestCategory("Testes de Unidade de Disciplina")]
public class SessaoTests
{
    private Sessao? sessao;

    [TestMethod]
    public void Deve_Gerar_Ingresso_Corretamente()
    {
        // Arrange
        var filme = new Filme("Filme Teste", 120, true, new GeneroFilme("Ação"));
        var sala = new Sala(1, 100);
        var sessao = new Sessao(DateTime.Now, 100, filme, sala);

        int assentoSelecionado = 10;
        bool meiaEntrada = true;

        // Act
        var ingresso = sessao.GerarIngresso(assentoSelecionado, meiaEntrada);

        // Assert
        Assert.AreEqual(ingresso.NumeroAssento, assentoSelecionado);
    }

    [TestMethod]
    public void Deve_Obter_Assentos_Disponiveis_Corretamente()
    {
        // Arrange
        var filme = new Filme("Filme Teste", 120, true, new GeneroFilme("Ação"));
        var sala = new Sala(1, 100);
        var sessao = new Sessao(DateTime.Now, 100, filme, sala);

        // Act
        var assentosDisponiveis = sessao.ObterAssentosDisponiveis();

        // Assert
        Assert.IsTrue(assentosDisponiveis.Length == 100);
    }

    [TestMethod]
    public void Deve_Obter_Quantidade_Ingressos_Disponiveis_Corretamente()
    {
        // Arrange
        var filme = new Filme("Filme Teste", 120, true, new GeneroFilme("Ação"));
        var sala = new Sala(1, 100);
        var sessao = new Sessao(DateTime.Now, 100, filme, sala);
        // Act
        var quantidadeDisponiveis = sessao.ObterQuantidadeIngressosDisponiveis();
        // Assert
        Assert.AreEqual(100, quantidadeDisponiveis);
    }
}
