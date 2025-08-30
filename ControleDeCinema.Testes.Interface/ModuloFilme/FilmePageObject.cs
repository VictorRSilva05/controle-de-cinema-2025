using ControleDeCinema.Testes.Interface.ModuloAutenticacao;
using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloFilme;
public class FilmeFormPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public FilmeFormPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));

        wait.Until(d => d.FindElement(By.CssSelector("form[data-se='formPrincipal']")).Displayed);
    }

    public FilmeFormPageObject PreencherTitulo(string titulo)
    {
        var inputNome = wait.Until(d => d.FindElement(By.Id("Titulo")));
        inputNome?.Clear();
        inputNome?.SendKeys(titulo);

        return this;
    }

    public FilmeFormPageObject PreencherDuracao(string duracao)
    {
        var inputNome = wait.Until(d => d.FindElement(By.Id("Duracao")));
        inputNome?.Clear();
        inputNome?.SendKeys(duracao);

        return this;
    }
    public FilmeFormPageObject SelecionarGenero(string tipo)
    {
        wait.Until(d =>
          d.FindElement(By.Id("GeneroId")).Displayed &&
          d.FindElement(By.Id("GeneroId")).Enabled
      );

        var select = new SelectElement(driver.FindElement(By.Id("GeneroId")));
        select.SelectByText(tipo);

        return this;
    }

    public FilmeIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();

        //wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return new FilmeIndexPageObject(driver!);
    }
}

public class FilmeIndexPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public FilmeIndexPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
    }

    public FilmeIndexPageObject IrPara(string enderecoBase)
    {
        // Use string interpolation to build the URL correctly for Razor Pages
        driver.Navigate().GoToUrl($"{enderecoBase}/filmes");

        // Wait for the page to load and the "Cadastrar" button to be visible
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return this;
    }

    public FilmeFormPageObject ClickCadastrar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();

        return new FilmeFormPageObject(driver!);
    }
    public FilmeFormPageObject ClickEditar()
    {
        wait.Until(d => d.FindElement(By.CssSelector(".card a[title='Edição']"))).Click();

        return new FilmeFormPageObject(driver!);
    }

    public FilmeFormPageObject ClickExcluir()
    {
        wait.Until(d => d.FindElement(By.CssSelector(".card a[title='Exclusão']"))).Click();

        return new FilmeFormPageObject(driver!);
    }

    public bool ContemFilme(string nome)
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return driver.PageSource.Contains(nome);
    }

    public bool ChamouExcecaoDeTitulo()
    {
        try
        {
            wait.Until(d => d.FindElement(By.CssSelector("span[data-se='spanTitulo']")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public bool ChamouExcecaoDeDuracao()
    {
        try
        {
            wait.Until(d => d.FindElement(By.CssSelector("span[data-se='spanDuracao']")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public bool ChamouExcecaoDeGenero()
    {
        try
        {
            wait.Until(d => d.FindElement(By.CssSelector("span[data-se='spanGenero']")));
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
