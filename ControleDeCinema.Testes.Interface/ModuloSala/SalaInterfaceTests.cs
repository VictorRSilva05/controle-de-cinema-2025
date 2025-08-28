﻿using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloAutenticacao;

namespace ControleDeCinema.Testes.Interface.ModuloSala;

[TestClass]
[TestCategory("Testes de Interface de Sala")]
public sealed class SalaInterfaceTests : TestFixture
{
    [TestInitialize]
    public override void InicializarTeste()
    {
        base.InicializarTeste();

        RegistrarContaEmpresarial();
    }

    [TestMethod]
    public void Deve_Cadastrar_Sala_Corretamente()
    {

        // Arrange
        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumero("1")
            .PreencherCapacidade("100")
            .Confirmar();

        // Assert
        Assert.IsTrue(salaIndex.ContemSala("1"));
    }

    [TestMethod]
    public void Deve_Editar_Sala_Corretamente()
    {
        // Arrange
        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumero("1")
            .PreencherCapacidade("100")
            .Confirmar();

        // Act
        salaIndex
            .ClickEditar()
            .PreencherNumero("2")
            .PreencherCapacidade("50")
            .Confirmar();

        // Assert
        Assert.IsTrue(salaIndex.ContemSala("2"));
    }

    [TestMethod]
    public void Deve_Excluir_Sala_Corretamente()
    {
        // Arrange
        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumero("1")
            .PreencherCapacidade("100")
            .Confirmar();

        // Act
        salaIndex
            .ClickExcluir()
            .Confirmar();

        // Assert
        Assert.IsFalse(salaIndex.ContemSala("# 1"));
    }

    [TestMethod]
    public void Deve_Listar_Salas_Corretamente()
    {
        // Arrange
        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        // Act
        salaIndex
            .ClickCadastrar()
            .PreencherNumero("1")
            .PreencherCapacidade("100")
            .Confirmar();

        salaIndex
          .ClickCadastrar()
          .PreencherNumero("2")
          .PreencherCapacidade("100")
          .Confirmar();

        // Assert
        Assert.IsTrue(salaIndex.ContemSala("2") && salaIndex.ContemSala("1"));
    }

    [TestMethod]
    public void Deve_Retornar_Erro_Caso_Campo_Esteja_Vazio()
    {
        // Arrange
        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        // Act
        salaIndex
            .ClickCadastrar()
            .Confirmar();

        // Assert
        Assert.IsTrue(salaIndex.ChamouExcecaoDeNumero());
    }

    [TestMethod]
    public void Deve_Retornar_Erro_Caso_Capacidade_For_Negativa()
    {
        // Arrange
        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumero("1")
            .PreencherCapacidade("-1")
            .Confirmar();


        // Assert
        Assert.IsTrue(salaIndex.ChamouExcecaoDeNumero());
    }

    [TestMethod]
    public void Deve_Retornar_Erro_Caso_Cadastro_For_Duplicado()
    {
        // Arrange
        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        // Act
        salaIndex
            .ClickCadastrar()
            .PreencherNumero("1")
            .PreencherCapacidade("100")
            .Confirmar();

        salaIndex
          .ClickCadastrar()
          .PreencherNumero("1")
          .PreencherCapacidade("100")
          .Confirmar();

        // Assert
        Assert.IsTrue(salaIndex.ChamouAlert());
    }

}
