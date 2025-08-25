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

        wait.Until(d => d.FindElement(By.CssSelector("form")).Displayed);
    }

    public SalaFormPageObject PreencherNumero(string numero)
    {
        var inputNumero = driver?.FindElement(By.Id("Numero"));
        inputNumero?.Clear();
        inputNumero?.SendKeys(numero);

        return this;
    }

    public SalaFormPageObject PreencherQuantidade(string capacidade)
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
        driver.Navigate().GoToUrl(Path.Combine(enderecoBase, "salas"));

        return this;
    }
    public SalaFormPageObject ClickCadastrar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();

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
}