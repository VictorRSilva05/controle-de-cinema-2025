using ControleDeCinema.Testes.Interface.ModuloAutenticacao;
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

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        wait.Until(d => d.FindElement(By.CssSelector("form")).Displayed);
    }

    public FilmeFormPageObject PreencherTitulo(string titulo)
    {
        var inputNome = driver?.FindElement(By.Id("Titulo"));
        inputNome?.Clear();
        inputNome?.SendKeys(titulo);

        return this;
    }

    public FilmeFormPageObject PreencherDuracao(string duracao)
    {
        var inputNome = driver?.FindElement(By.Id("Duracao"));
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
        wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']"))).Click();

        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

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

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public FilmeIndexPageObject IrPara(string enderecoBase)
    {
        driver.Navigate().GoToUrl(Path.Combine(enderecoBase, "filmes"));

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
}
