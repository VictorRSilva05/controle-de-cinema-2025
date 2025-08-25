using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Org.BouncyCastle.Asn1.Mozilla;

namespace ControleDeCinema.Testes.Interface.ModuloAutenticacao;
public class AutenticacaoFormPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public AutenticacaoFormPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        wait.Until(d => d.FindElement(By.CssSelector("form")).Displayed);
    }

    public AutenticacaoFormPageObject PreencherEmail(string email)
    {
        wait.Until(d =>
            d.FindElement(By.Id("Email")).Displayed &&
            d.FindElement(By.Id("Email")).Enabled
        );

        var inputEmail = driver.FindElement(By.Id("Email"));
        inputEmail.Clear();
        inputEmail.SendKeys(email);

        return this;
    }

    public AutenticacaoFormPageObject PreencherSenha(string senha)
    {
        wait.Until(d =>
            d.FindElement(By.Id("Senha")).Displayed &&
            d.FindElement(By.Id("Senha")).Enabled
        );

        var inputSenha = driver.FindElement(By.Id("Senha"));
        inputSenha.Clear();
        inputSenha.SendKeys(senha);

        return this;
    }

    public AutenticacaoFormPageObject ConfirmarSenha(string senha)
    {
        wait.Until(d =>
           d.FindElement(By.Id("ConfirmarSenha")).Displayed &&
           d.FindElement(By.Id("ConfirmarSenha")).Enabled
       );

        var inputSenha = driver.FindElement(By.Id("ConfirmarSenha"));
        inputSenha.Clear();
        inputSenha.SendKeys(senha);

        return this;
    }

    public AutenticacaoFormPageObject SelecionarTipoDeUsuario(string tipo)
    {
        wait.Until(d =>
          d.FindElement(By.Id("Tipo")).Displayed &&
          d.FindElement(By.Id("Tipo")).Enabled
      );

        var select = new SelectElement(driver.FindElement(By.Id("Tipo")));
        select.SelectByText(tipo);

        return this;
    }

    public AutenticacaoIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']"))).Click();

        return new AutenticacaoIndexPageObject(driver);
    }
}

public class AutenticacaoIndexPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public AutenticacaoIndexPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public AutenticacaoFormPageObject ClickCadastrar()
    {
        wait.Until(d => d?.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();

        return new AutenticacaoFormPageObject(driver!);
    }

    public AutenticacaoIndexPageObject IrPara(string enderecoBase)
    {
        driver.Navigate().GoToUrl(Path.Combine(enderecoBase, "autenticacao/login"));
        return this;
    }
}
