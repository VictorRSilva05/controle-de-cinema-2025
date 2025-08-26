using ControleDeCinema.Dominio.ModuloGeneroFilme;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
public class GeneroFilmeFormPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public GeneroFilmeFormPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        wait.Until(d => d.FindElement(By.CssSelector("form")).Displayed);
    }

    public GeneroFilmeFormPageObject PreencherNome(string nome)
    {
        var inputNome = wait.Until(d => d.FindElement(By.CssSelector("a[data-se='inputDescricao']")));
       
        inputNome?.Clear();
        inputNome?.SendKeys(nome);

        return this;
    }

    public GeneroFilmeIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']"))).Click();

        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return new GeneroFilmeIndexPageObject(driver!);
    }
}

public class GeneroFilmeIndexPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public GeneroFilmeIndexPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public GeneroFilmeIndexPageObject IrPara(string enderecoBase)
    {
        driver.Navigate().GoToUrl(Path.Combine(enderecoBase, "generos"));

        return this;
    }
    public GeneroFilmeFormPageObject ClickCadastrar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();

        return new GeneroFilmeFormPageObject(driver!);
    }

    public GeneroFilmeFormPageObject ClickEditar()
    {
        wait.Until(d => d.FindElement(By.CssSelector(".card a[title='Edição']"))).Click();

        return new GeneroFilmeFormPageObject(driver!);
    }

    public GeneroFilmeFormPageObject ClickExcluir()
    {
        wait.Until(d => d.FindElement(By.CssSelector(".card a[title='Exclusão']"))).Click();

        return new GeneroFilmeFormPageObject(driver!);
    }

    public bool ContemGeneroFilme(string nome)
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return driver.PageSource.Contains(nome);
    }
}
