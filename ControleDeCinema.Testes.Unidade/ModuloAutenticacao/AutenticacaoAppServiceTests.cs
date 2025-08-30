using ControleDeCinema.Aplicacao.ModuloAutenticacao;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using FluentResults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ControleDeCinema.Testes.Aplicacao.ModuloAutenticacao
{
    [TestClass]
    public class AutenticacaoAppServiceTests
    {
        private Mock<UserManager<Usuario>> userManagerMock;
        private Mock<RoleManager<Cargo>> roleManagerMock;
        private FakeSignInManagerBase signInManagerFake;
        private AutenticacaoAppService service;

        [TestInitialize]
        public void Setup()
        {
            // Mock do UserManager
            userManagerMock = new Mock<UserManager<Usuario>>(
                new Mock<IUserStore<Usuario>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<Usuario>>().Object,
                Array.Empty<IUserValidator<Usuario>>(),
                Array.Empty<IPasswordValidator<Usuario>>(),
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<Usuario>>>().Object
            );

            // Mock do RoleManager
            roleManagerMock = new Mock<RoleManager<Cargo>>(
                new Mock<IRoleStore<Cargo>>().Object,
                Array.Empty<IRoleValidator<Cargo>>(),
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<ILogger<RoleManager<Cargo>>>().Object
            );

            // Fake SignInManager (controlamos o resultado via .Resultado)
            signInManagerFake = new FakeSignInManagerBase(userManagerMock.Object);

            // Instancia do serviço
            service = new AutenticacaoAppService(
                userManagerMock.Object,
                signInManagerFake,
                roleManagerMock.Object
            );
        }

        [TestMethod]
        public async Task RegistrarAsync_DeveCriarUsuarioComSucesso()
        {
            var usuario = new Usuario { UserName = "teste", Email = "teste@teste.com" };

            userManagerMock.Setup(x => x.CreateAsync(usuario, "123456"))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock.Setup(x => x.AddToRoleAsync(usuario, It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((Cargo)null);

            roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<Cargo>()))
                .ReturnsAsync(IdentityResult.Success);

            signInManagerFake.Resultado = SignInResult.Success;

            var result = await service.RegistrarAsync(usuario, "123456", TipoUsuario.Empresa);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task RegistrarAsync_DeveFalhar_QuandoUsuarioJaExiste()
        {
            var usuario = new Usuario { UserName = "teste", Email = "teste@teste.com" };

            userManagerMock.Setup(x => x.CreateAsync(usuario, "123456"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "DuplicateUserName" }));

            var result = await service.RegistrarAsync(usuario, "123456", TipoUsuario.Empresa);

            Assert.IsTrue(result.IsFailed);
        }

        [TestMethod]
        public async Task LoginAsync_DeveFuncionar_QuandoCredenciaisCorretas()
        {
            signInManagerFake.Resultado = SignInResult.Success;

            var result = await service.LoginAsync("teste@teste.com", "123456");

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task LoginAsync_DeveFalhar_QuandoCredenciaisInvalidas()
        {
            signInManagerFake.Resultado = SignInResult.Failed;

            var result = await service.LoginAsync("teste@teste.com", "senhaErrada");

            Assert.IsTrue(result.IsFailed);
        }

        [TestMethod]
        public async Task LogoutAsync_DeveChamarSignOut()
        {
            var result = await service.LogoutAsync();

            Assert.IsTrue(result.IsSuccess);
        }
    }

    // ✅ Fake SignInManager simplificado
    public class FakeSignInManagerBase : SignInManager<Usuario>
    {
        public FakeSignInManagerBase(UserManager<Usuario> userManager)
            : base(userManager,
                   new Mock<IHttpContextAccessor>().Object,
                   new Mock<IUserClaimsPrincipalFactory<Usuario>>().Object,
                   new Mock<IOptions<IdentityOptions>>().Object,
                   new Mock<ILogger<SignInManager<Usuario>>>().Object,
                   new Mock<IAuthenticationSchemeProvider>().Object)
        { }

        public SignInResult Resultado { get; set; } = SignInResult.Success;

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
            => Task.FromResult(Resultado);

        public override Task SignOutAsync()
            => Task.CompletedTask;
    }
}
