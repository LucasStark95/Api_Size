using Size.Core.Models;
using Size.Core.Request;
using Size.Core.Responses;
using Size.Testes.Context;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Size.Testes.Integracao.Api
{
    public class ContaControllerTestes
    {
        private readonly TestContext _testContext;
        private readonly string _documento;
        private readonly string _nome;

        public ContaControllerTestes()
        {
            _testContext = new TestContext();

            _nome = "Antonio Lucas";
            _documento = "1745896547899";
        }

        [Fact]
        public async Task TestaUmDepositoValido()
        {
            //Arrange
            var valor = 2000;
            var deposito = new MovementRequest() { Documento = _documento, Valor = valor};

            var cliente = new Cliente() { Nome = _nome, Documento = _documento };
            var login = new Login() { Nome = _nome, Documento = _documento };

            var clienteResponse = await _testContext.Client.PostAsync("api/cliente", cliente, new JsonMediaTypeFormatter());
            var token = await GetTokenResponseAsync(login);

            _testContext.Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Token);

            //Act
            var depositoResponse = await _testContext.Client.PostAsync("api/conta/deposito", deposito, new JsonMediaTypeFormatter());
            depositoResponse.EnsureSuccessStatusCode();


            //Assert
            Assert.Equal(HttpStatusCode.OK, depositoResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, clienteResponse.StatusCode);
            Assert.NotNull(token.Token);
        }

        [Fact]
        public async Task TestaUmSaqueValido()
        {
            //Arrange
            var valor = 500;
            var saque = new MovementRequest() { Documento = _documento, Valor = valor };

            var cliente = new Cliente() { Nome = _nome, Documento = _documento, Conta = new Conta { Saldo = 2000 } };
            var login = new Login() { Nome = _nome, Documento = _documento };

            var clienteResponse = await _testContext.Client.PostAsync("api/cliente", cliente, new JsonMediaTypeFormatter());
            var token = await GetTokenResponseAsync(login);

            _testContext.Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Token);

            //Act
            var depositoResponse = await _testContext.Client.PostAsync("api/conta/saque", saque, new JsonMediaTypeFormatter());
            depositoResponse.EnsureSuccessStatusCode();


            //Assert
            Assert.Equal(HttpStatusCode.OK, depositoResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, clienteResponse.StatusCode);
            Assert.NotNull(token.Token);
        }

        [Fact]
        public async Task TestaUmExtratoValido()
        {
            //Arrange
            var cliente = new Cliente() { Nome = _nome, Documento = _documento };
            var login = new Login() { Nome = _nome, Documento = _documento };

            var clienteResponse = await _testContext.Client.PostAsync("api/cliente", cliente, new JsonMediaTypeFormatter());
            var token = await GetTokenResponseAsync(login);

            _testContext.Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Token);

            //Act
            var depositoResponse = await _testContext.Client.GetAsync("api/conta/extrato/"+_documento);
            depositoResponse.EnsureSuccessStatusCode();


            //Assert
            Assert.Equal(HttpStatusCode.OK, depositoResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, clienteResponse.StatusCode);
            Assert.NotNull(token.Token);
        }

        private async Task<TokenResponse> GetTokenResponseAsync(Login login)
        {
            try
            {
                TokenResponse result = null;
                var response = _testContext.Client.PostAsync("api/account", login, new JsonMediaTypeFormatter()).ContinueWith(task =>
                {
                    var r = task.Result;
                    var jsonString = r.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    result = JsonSerializer.Deserialize<TokenResponse>(jsonString.Result,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                });

                await response;

                return result;
            }
            catch
            {
                throw new Exception();
            }

        }
    }
    
}
