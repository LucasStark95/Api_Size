using Size.Core.Models;
using Size.Core.Request;
using Size.Testes.Context;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Xunit;

namespace Size.Testes.Integracao.Api
{
    public class AccountControllerTestes
    {
        private readonly TestContext _testContext;

        public AccountControllerTestes()
        {
            _testContext = new TestContext();
        }
        
        [Fact]
        public async Task BuscaTokenClienteAutorizado()
        {
            //Arrange
            var nome = "Antonio Lucas";
            var documento = "005488566";

            var cliente = new Cliente() { Nome = nome, Documento = documento };
            var login = new Login() { Nome = nome, Documento = documento };

            //Act
            var clienteResponse = await _testContext.Client.PostAsync("api/cliente", cliente, new JsonMediaTypeFormatter());
            var tokenResponse = await _testContext.Client.PostAsync("api/account", login, new JsonMediaTypeFormatter());

            clienteResponse.EnsureSuccessStatusCode();
            tokenResponse.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, clienteResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, tokenResponse.StatusCode);
        }
        
        [Fact]
        public async Task BuscaTokenClienteNaoAutorizado()
        {
            //Arrange
            var nome = "Antonio Lucas";
            var documento = "0054885748";
            var login = new Login() { Nome = nome, Documento = documento };

            //Act
            var tokenResponse = await _testContext.Client.PostAsync("api/account", login, new JsonMediaTypeFormatter());
          
            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, tokenResponse.StatusCode);
        }

        [Fact]
        public async Task BuscaTokenLoginNulo()
        {
            //Arrange
            var login = new Login();

            //Act
            var tokenResponse = await _testContext.Client.PostAsync("api/account", login, new JsonMediaTypeFormatter());

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, tokenResponse.StatusCode);
        }
    }
}
