using ControleDeCinema.Infraestrutura.Orm.Compartilhado;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace TesteFacil.Testes.Interface.Compartilhado;

[TestClass]
public abstract class TestFixtureLocal
{
    protected static IWebDriver? driver;
    protected ControleDeCinemaDbContext? dbContext;

    protected static string enderecoBase = "https://localhost:7131";
    private static string connectionString = "Host=localhost;Port=5432;Database=controle-de-cinema-selenium-local;Username=postgres;Password=YourStrongPassword";

    [TestInitialize]
    public void ConfigurarTestes()
    {
        dbContext = ControleDeCinemaDbContextFactory.CriarDbContext(connectionString);

        ConfigurarTabelas(dbContext);

        InicializarWebDriver();
    }

    [TestCleanup]
    public void Cleanup()
    {
        EncerrarWebDriver();
    }

    private static void InicializarWebDriver()
    {
        var options = new FirefoxOptions();

        driver = new FirefoxDriver(options);
    }

    private static void EncerrarWebDriver()
    {
        driver?.Quit();
        driver?.Dispose();
    }

    [TestInitialize]
    public virtual void InicializarTeste()
    {
        ConfigurarTabelas(dbContext);

        driver?.Manage().Cookies.DeleteAllCookies();
    }

    private static void ConfigurarTabelas(ControleDeCinemaDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        dbContext.Sessoes.RemoveRange(dbContext.Sessoes);
        dbContext.Ingressos.RemoveRange(dbContext.Ingressos);
        dbContext.Filmes.RemoveRange(dbContext.Filmes);
        dbContext.GenerosFilme.RemoveRange(dbContext.GenerosFilme);
        dbContext.Salas.RemoveRange(dbContext.Salas);
        dbContext.Users.RemoveRange(dbContext.Users);
        dbContext.Roles.RemoveRange(dbContext.Roles);

        dbContext.SaveChanges();
    }

    protected static void RegistrarContaEmpresarial()
    {
        driver?.Navigate().GoToUrl($"{enderecoBase}/autenticacao/registro");

        IWebElement inputEmail = driver.FindElement(By.Id("Email"));
        IWebElement inputSenha = driver.FindElement(By.Id("Senha"));
        IWebElement inputConfirmarSenha = driver.FindElement(By.Id("ConfirmarSenha"));
        SelectElement selectTipoUsuario = new(driver.FindElement(By.Id("Tipo")));

        inputEmail.Clear();
        inputEmail.SendKeys("tioguda@gmail.com");

        inputSenha.Clear();
        inputSenha.SendKeys("abcBolinhas12345");

        inputConfirmarSenha.Clear();
        inputConfirmarSenha.SendKeys("abcBolinhas12345");

        selectTipoUsuario.SelectByText("Empresa");

        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(20));

        wait.Until(d =>
        {
            IWebElement btn = d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"));
            if (!btn.Enabled || !btn.Displayed) return false;
            btn.Click();
            return true;
        });

        wait.Until(d =>
            !d.Url.Contains("/autenticacao/registro", StringComparison.OrdinalIgnoreCase) &&
            d.FindElements(By.CssSelector("form[action='/autenticacao/registro']")).Count == 0
        );

        wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
    }
}