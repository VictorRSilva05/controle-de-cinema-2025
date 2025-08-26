using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ControleDeCinema.Testes.Interface.ModuloSala;
public class SalaFormPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public SalaFormPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

       //wait.Until(d => d.FindElement(By.CssSelector("form")).Displayed);
    }

    public SalaFormPageObject PreencherNumero(string numero)
    {
        var inputNumero = driver?.FindElement(By.Id("Numero"));
        inputNumero?.Clear();
        inputNumero?.SendKeys(numero);

        return this;
    }

    public SalaFormPageObject PreencherCapacidade(string capacidade)
    {
        var inputCapacidade = driver?.FindElement(By.Id("Capacidade"));
        inputCapacidade?.Clear();
        inputCapacidade?.SendKeys(capacidade);

        return this;
    }

    public SalaIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']"))).Click();

        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return new SalaIndexPageObject(driver!);
    }
}

public class SalaIndexPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public SalaIndexPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public SalaIndexPageObject IrPara(string enderecoBase)
    {
        // Use string interpolation to build the URL correctly for Razor Pages
        driver.Navigate().GoToUrl($"{enderecoBase}/salas");

        // Wait for the page to load and the "Cadastrar" button to be visible
       wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return this;
    }
    public SalaFormPageObject ClickCadastrar()
    {
        //wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();

        driver.FindElement(By.Id("btnCadastrar")).Click();
        return new SalaFormPageObject(driver!);
    }

    public SalaFormPageObject ClickEditar()
    {
        wait.Until(d => d.FindElement(By.CssSelector(".card a[title='Edição']"))).Click();

        return new SalaFormPageObject(driver!);
    }

    public SalaFormPageObject ClickExcluir()
    {
        wait.Until(d => d.FindElement(By.CssSelector(".card a[title='Exclusão']"))).Click();

        return new SalaFormPageObject(driver!);
    }

    public bool ContemSala(string sala)
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return driver.PageSource.Contains(sala);
    }

    public bool ContemBotaoCadastrar()
    {
        try
        {
            wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public bool ContemCampoNumero()
    {
        try
        {
            wait.Until(d => d.FindElement(By.Id("Numero")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}