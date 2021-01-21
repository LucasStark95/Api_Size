using Microsoft.EntityFrameworkCore;
using Size.Core.Models;
using Size.Core.Request;
using Size.Data.EFCore.Context;
using Size.Data.EFCore.Repositorios;
using System.Threading.Tasks;
using Xunit;

namespace Size.Testes.Unitario.Data
{
    public class ClienteRepositorioTestes
    {
        private readonly Contexto _context;
        private readonly ClienteRepositorio _repository;

        public ClienteRepositorioTestes()
        {
            var options = new DbContextOptionsBuilder<Contexto>()
               .UseInMemoryDatabase(databaseName: "Size")
               .Options;

            _context = new Contexto(options);
            _repository = new ClienteRepositorio(_context);
        }

        [Fact]
        public async Task TestaACriacaoDeUmNovoCliente()
        {
            //Arrange
            var clienteMock = new Cliente() { Nome = "Antonio Lucas", Documento = "000000000000"};

            //Act
            var cliente = await _repository.AdicionarAsync(clienteMock);

            //Assert
            Assert.NotNull(cliente);
            Assert.Equal(clienteMock.Nome, cliente.Nome);
            Assert.Equal(clienteMock.Documento, cliente.Documento);
        }
        
        [Fact]
        public async Task TestaABuscaClientePorLogin()
        {
            //Arrange
            var clienteMock = new Cliente() { Nome = "Antonio L. Almeida", Documento = "000000000000" };
            
            await _context.Clientes.AddAsync(clienteMock);
            await _context.SaveChangesAsync();

            var login = new Login() { Nome = "Antonio L. Almeida", Documento = "000000000000" };

            //Act
            var cliente = await _repository.GetClienteByLogin(login);

            //Assert
            Assert.NotNull(cliente);
            Assert.Equal(clienteMock.Nome, cliente.Nome);
            Assert.Equal(clienteMock.Documento, cliente.Documento);

        }
        
        [Fact]
        public async Task TestaABuscaClientePorDocumento()
        {
            //Arrange
            var documento = "456456456";
            var clienteMock = new Cliente() { Nome = "Antonio Almeida", Documento = documento };

            await _context.Clientes.AddAsync(clienteMock);
            await _context.SaveChangesAsync();

            //Act
            var cliente = await _repository.GetClienteByDocumentoAsync(documento);

            //Assert
            Assert.NotNull(cliente);
            Assert.Equal(clienteMock.Nome, cliente.Nome);
            Assert.Equal(clienteMock.Documento, cliente.Documento);
        }
        
        [Fact]
        public async Task TestaABuscaClienteInexistente()
        {
            //Arrange
            var documento = "101010101010";

            //Act
            var cliente = await _repository.GetClienteByDocumentoAsync(documento);

            //Assert
            Assert.Null(cliente);
        }
       
    }
}
