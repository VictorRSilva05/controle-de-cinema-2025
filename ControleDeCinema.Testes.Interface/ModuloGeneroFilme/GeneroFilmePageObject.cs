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

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));

        wait.Until(d => d.FindElement(By.CssSelector("form[data-se='formPrincipal']")).Displayed);
    }

    public GeneroFilmeFormPageObject PreencherNome(string nome)
    {
        var inputNome = wait.Until(d => d.FindElement(By.Id("Descricao")));
       
        inputNome?.Clear();
        inputNome?.SendKeys(nome);

        return this;
    }

    public GeneroFilmeIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();

        //wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

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

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
    }

    public GeneroFilmeIndexPageObject IrPara(string enderecoBase)
    {
        // Use string interpolation to build the URL correctly for Razor Pages
        driver.Navigate().GoToUrl($"{enderecoBase}/generos");

        // Wait for the page to load and the "Cadastrar" button to be visible
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

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

    public bool ChamouExcecaoDeDescricao()
    {
        try
        {
            wait.Until(d => d.FindElement(By.CssSelector("span[data-se='spanDescricao']")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public bool ChamouAlert()
    {
        try
        {
            wait.Until(d => d.FindElement(By.CssSelector("div[data-se='alert']")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}
