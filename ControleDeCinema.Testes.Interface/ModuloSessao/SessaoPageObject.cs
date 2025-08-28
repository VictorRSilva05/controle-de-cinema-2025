using ControleDeCinema.Testes.Interface.ModuloFilme;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ControleDeCinema.Testes.Interface.ModuloSessao;
public class SessaoFormPageObject 
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public SessaoFormPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));

        wait.Until(d => d.FindElement(By.CssSelector("form[data-se='formPrincipal']")).Displayed);
    }

    public SessaoFormPageObject PreecherInicio(string data)
    {
        var input = wait.Until(d => d.FindElement(By.Id("Inicio")));
        input?.Clear();
        input?.SendKeys(data);

        return this;
    }

    public SessaoFormPageObject PreencherIngressos(string ingressos)
    {
        var input = wait.Until(d => d.FindElement(By.Id("NumeroMaximoIngressos")));
        input?.Clear();
        input?.SendKeys(ingressos);

        return this;
    }

    public SessaoFormPageObject SelecionarFilme(string tipo)
    {
        wait.Until(d =>
          d.FindElement(By.Id("FilmeId")).Displayed &&
          d.FindElement(By.Id("FilmeId")).Enabled
      );

        var select = new SelectElement(driver.FindElement(By.Id("FilmeId")));
        select.SelectByText(tipo);

        return this;
    }

    public SessaoFormPageObject SelecionarSala(string tipo)
    {
        wait.Until(d =>
          d.FindElement(By.Id("SalaId")).Displayed &&
          d.FindElement(By.Id("SalaId")).Enabled
      );

        var select = new SelectElement(driver.FindElement(By.Id("SalaId")));
        select.SelectByText(tipo);

        return this;
    }

    public SessaoIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();

        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return new SessaoIndexPageObject(driver!);
    }
}

public class SessaoIndexPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public SessaoIndexPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
    }

    public SessaoIndexPageObject IrPara(string enderecoBase)
    {
        // Use string interpolation to build the URL correctly for Razor Pages
        driver.Navigate().GoToUrl($"{enderecoBase}/sessoes");

        // Wait for the page to load and the "Cadastrar" button to be visible
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return this;
    }

    public SessaoFormPageObject ClickCadastrar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();

        return new SessaoFormPageObject(driver!);
    }
    public SessaoFormPageObject ClickEditar()
    {
        wait.Until(d => d.FindElement(By.CssSelector(".card a[title='Edição']"))).Click();

        return new SessaoFormPageObject(driver!);
    }

    public SessaoFormPageObject ClickExcluir()
    {
        wait.Until(d => d.FindElement(By.CssSelector(".card a[title='Exclusão']"))).Click();

        return new SessaoFormPageObject(driver!);
    }

    public bool ContemSessao(string nome)
    {
        //wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return driver.PageSource.Contains(nome);
    }
}