using Size.Core.Models;
using Size.Testes.Context;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Xunit;

namespace Size.Testes.Integracao.Api
{
    public class ClienteControllerTestes
    {
        private readonly TestContext _testContext;

        public ClienteControllerTestes()
        {
            _testContext = new TestContext();
        }

        [Fact]
        public async Task TestaACriacaoDeUmCliente()
        {
            //Arrange
            var nome = "Antonio Lucas";
            var documento = "1123456689787";
            var cliente = new Cliente() { Nome = nome, Documento = documento };
            
            //Act
            var clienteResponse = await _testContext.Client.PostAsync("api/cliente", cliente, new JsonMediaTypeFormatter());
            clienteResponse.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.OK, clienteResponse.StatusCode);
            
        }
        
        [Fact]
        public async Task TestaACriacaoDeUmClienteSemNome()
        {
            //Arrange
            var documento = "1123456689787";
            var cliente = new Cliente() { Documento = documento };

            //Act
            var clienteResponse = await _testContext.Client.PostAsync("api/cliente", cliente, new JsonMediaTypeFormatter());

            //Assert
            Assert.Equal(HttpStatusCode.PreconditionFailed, clienteResponse.StatusCode);

        }
        
        [Fact]
        public async Task TestaACriacaoDeUmClienteSemDocumento()
        {
            //Arrange
            var nome = "Antonio Lucas";
            var cliente = new Cliente() { Nome = nome };

            //Act
            var clienteResponse = await _testContext.Client.PostAsync("api/cliente", cliente, new JsonMediaTypeFormatter());

            //Assert
            Assert.Equal(HttpStatusCode.PreconditionFailed, clienteResponse.StatusCode);

        }

        [Fact]
        public async Task TestaACriacaoDeUmClienteNUlo()
        {
            //Arrange
            var cliente = new Cliente();

            //Act
            var clienteResponse = await _testContext.Client.PostAsync("api/cliente", cliente, new JsonMediaTypeFormatter());

            //Assert
            Assert.Equal(HttpStatusCode.PreconditionFailed, clienteResponse.StatusCode);

        }
    }
}
