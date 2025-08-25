using ControleDeCinema.Testes.Interface.Compartilhado;

namespace ControleDeCinema.Testes.Interface.ModuloAutenticacao;

[TestClass]
[TestCategory("Testes de Interface de Autenticacao")]
public sealed class AutenticacaoInterfaceTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Empresa_Corretamente()
    {
        // Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObject(driver!)
             .IrPara(enderecoBase!);

        // Act
        autenticacaoIndex
            .ClickCadastrar()
            .PreencherEmail("h.romeupinto@gmail.com")
            .PreencherSenha("AbcBolinhas12345")
            .ConfirmarSenha("AbcBolinhas12345")
            .SelecionarTipoDeUsuario("Empresa")
            .Confirmar();

        // Assert
        Assert.AreEqual("/", driver!.Url);
    }

    [TestMethod]
    public void Deve_Cadastrar_Cliente_Corretamente()
    {
        // Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        // Act
        autenticacaoIndex
            .ClickCadastrar()
            .PreencherEmail("jefferson.caminhoes@hotmail.com")
            .PreencherSenha("AbcBolinhas12345")
            .ConfirmarSenha("AbcBolinhas12345")
            .SelecionarTipoDeUsuario("Cliente")
            .Confirmar();

        // Assert
    }

    [TestMethod]
    public void Deve_Realizar_Login_Corretamente()
    {
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Ao_Realizar_Login_Com_Crendiciais_Invalidas()
    {
    }

    [TestMethod]
    public void Deve_Realizar_Logout_Corretamente()
    {
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Ao_Informar_Um_Email_Ja_Cadastrado()
    {

    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Ao_Nao_Confirmar_Senha()
    {
        // Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObject(driver!)
             .IrPara(enderecoBase!);

        // Act
        autenticacaoIndex
            .ClickCadastrar()
            .PreencherEmail("paulatejano@outlook.com")
            .PreencherSenha("AbcBolinhas12345")
            .SelecionarTipoDeUsuario("Cliente")
            .Confirmar();

        // Assert
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Ao_Nao_Informar_ASenha()
    {
        // Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObject(driver!)
             .IrPara(enderecoBase!);

        // Act
        autenticacaoIndex
            .ClickCadastrar()
            .PreencherEmail("jailimrabei@outlook.com")
            .SelecionarTipoDeUsuario("Cliente")
            .Confirmar();

        // Assert
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Ao_Nao_Informar_OEmail()
    {
        // Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObject(driver!)
             .IrPara(enderecoBase!);

        // Act
        autenticacaoIndex
            .ClickCadastrar()
            .PreencherSenha("AbcBolinhas12345")
            .ConfirmarSenha("AbcBolinhas12345")
            .SelecionarTipoDeUsuario("Cliente")
            .Confirmar();

        // Assert
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Ao_Inserir_Formato_De_Email_Invalido()
    {
        // Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObject(driver!)
             .IrPara(enderecoBase!);

        // Act
        autenticacaoIndex
            .ClickCadastrar()
            .PreencherEmail("tonigarrido.com")
            .PreencherSenha("AbcBolinhas12345")
            .ConfirmarSenha("AbcBolinhas12345")
            .SelecionarTipoDeUsuario("Cliente")
            .Confirmar();

        // Assert
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Ao_Inserir_Senha_Com_Tamanho_Invalido()
    {
        // Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObject(driver!)
             .IrPara(enderecoBase!);

        // Act
        autenticacaoIndex
            .ClickCadastrar()
            .PreencherEmail("benjamin.arrola@msn.com")
            .PreencherSenha("Ab5")
            .ConfirmarSenha("Ab5")
            .SelecionarTipoDeUsuario("Cliente")
            .Confirmar();

        // Assert
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Ao_Inserir_Senha_Somente_Com_Minusculas()
    {
        // Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObject(driver!)
             .IrPara(enderecoBase!);

        // Act
        autenticacaoIndex
            .ClickCadastrar()
            .PreencherEmail("benjamin.arrola@msn.com")
            .PreencherSenha("abcbolinhas12345")
            .ConfirmarSenha("abcbolinhas12345")
            .SelecionarTipoDeUsuario("Cliente")
            .Confirmar();

        // Assert
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Ao_Inserir_Senha_Sem_Numeros()
    {
        // Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObject(driver!)
             .IrPara(enderecoBase!);

        // Act
        autenticacaoIndex
            .ClickCadastrar()
            .PreencherEmail("benjamin.arrola@msn.com")
            .PreencherSenha("abcBolinhas")
            .ConfirmarSenha("abcBolinhas")
            .SelecionarTipoDeUsuario("Cliente")
            .Confirmar();

        // Assert
    }

    [TestMethod]
    public void Deve_Ocorrer_Erro_Se_As_Senhas_Nao_Forem_Iguais()
    {
        // Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObject(driver!)
             .IrPara(enderecoBase!);

        // Act
        autenticacaoIndex
            .ClickCadastrar()
            .PreencherEmail("benjamin.arrola@msn.com")
            .PreencherSenha("abcBolinhas12345")
            .ConfirmarSenha("abcBolinhas54321")
            .SelecionarTipoDeUsuario("Cliente")
            .Confirmar();

        // Assert
    }
}