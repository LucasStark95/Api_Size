using Microsoft.EntityFrameworkCore;
using Size.Core.Models;
using Size.Data.EFCore.Context;
using Size.Data.EFCore.Repositorios;
using System.Threading.Tasks;
using Xunit;

namespace Size.Testes.Unitario.Data
{
    public class ContaRepositorioTestes
    {
        private readonly Contexto _context;
        private readonly ContaRepositorio _repository;
        private readonly ClienteRepositorio _repositorioCliente;

        public ContaRepositorioTestes()
        {
            var options = new DbContextOptionsBuilder<Contexto>()
               .UseInMemoryDatabase(databaseName: "Size")
               .Options;

            _context = new Contexto(options);
            _repository = new ContaRepositorio(_context);
            _repositorioCliente = new ClienteRepositorio(_context);
        }

        [Fact]
        public async Task TestaOdepositoEmUmaConta()
        {
            //Arrange
            var documento = "123456789101";
            var clienteMock = new Cliente() { Nome = "Antonio L. Almeida", Documento = documento, Conta = new Conta() };
            var deposito = 2000;

            await _context.Clientes.AddAsync(clienteMock);
            await _context.SaveChangesAsync();

            //Act
            var cliente = await _repositorioCliente.GetClienteByDocumentoAsync(documento);
            var conta = await _repository.DepositaAsync(cliente.Id, deposito);

            //Assert
            Assert.NotNull(cliente);
            Assert.NotNull(conta);
            Assert.Equal(deposito, conta.Saldo);
        }
        
        [Fact]
        public async Task TestaOdepositoEmUmaContaInexistente()
        {
            //Arrange
            var clienteFakeId = 100;
            var deposito = 2000;

            //Act
            var record = await Record.ExceptionAsync(async () => await _repository.DepositaAsync(clienteFakeId, deposito));

            //Assert
            Assert.NotNull(record);

        }
        
        [Fact]
        public async Task TestaOSaque()
        {
            //Arrange
            var documento = "123456789";
            var clienteMock = new Cliente() { Nome = "Antonio Almeida", Documento = documento, Conta = new Conta() { Saldo = 2500 }  };
            var saque = 500;

            await _context.Clientes.AddAsync(clienteMock);
            await _context.SaveChangesAsync();

            //Act
            var cliente = await _repositorioCliente.GetClienteByDocumentoAsync(documento);
            var conta = await _repository.SaqueAsync(cliente.Id, saque);

            //Assert
            Assert.NotNull(cliente);
            Assert.NotNull(conta);
            Assert.Equal(cliente.Conta.Saldo, conta.Saldo);

        }
        
        
        [Fact]
        public async Task TestaOSaqueEmUmaContaInexistente()
        {
            //Arrange
            var clienteFakeId = 100;
            var saque = 500;

            //Act
            var record = await Record.ExceptionAsync(async () => await _repository.SaqueAsync(clienteFakeId, saque));

            //Assert
            Assert.NotNull(record);
            
        }
        
        [Fact]
        public async Task TestaOExtratoEmUmaConta()
        {
            //Arrange
            var documento = "123456789145";
            var clienteMock = new Cliente() { Nome = "Antonio Almeida", Documento = documento, Conta = new Conta() { Saldo = 2500 } };
           

            await _context.Clientes.AddAsync(clienteMock);
            await _context.SaveChangesAsync();

            //Act
            var cliente = await _repositorioCliente.GetClienteByDocumentoAsync(documento);
            var conta = await _repository.ExtratoAsync(cliente.Id);

            //Assert
            Assert.NotNull(cliente);
            Assert.NotNull(conta);

        }
        
        [Fact]
        public async Task TestaOExtratoEmUmaContaInexistente()
        {
            //Arrange
            var clienteIdFake = 500;
           
            //Act
            var conta = await _repository.ExtratoAsync(clienteIdFake);

            //Assert
            Assert.Null(conta);

        }
        
        [Fact]
        public async Task TestaATransferenciaEmUmaConta()
        {
            //Arrange
            var documentoRt = "12345678914563";
            var clienteRtMock = new Cliente() { Nome = "Antonio Almeida", Documento = documentoRt, Conta = new Conta() { Saldo = 2500 } }; 

            var documentoDt = "1234567891789";
            var clienteDtMock = new Cliente() { Nome = "Antonio L Almeida", Documento = documentoDt, Conta = new Conta() { Saldo = 1500 } };

            var transferencia = 1500;


            await _context.Clientes.AddAsync(clienteRtMock);
            await _context.Clientes.AddAsync(clienteDtMock);
            await _context.SaveChangesAsync();

            //Act
            var clienteRt = await _repositorioCliente.GetClienteByDocumentoAsync(documentoRt);
            var clienteDt = await _repositorioCliente.GetClienteByDocumentoAsync(documentoDt);
            var operacao = await _repository.TransferirAsync(clienteRt.Id, clienteDt.Id, transferencia);

            //Assert
            Assert.NotNull(clienteRt);
            Assert.NotNull(clienteDt);

            Assert.True(operacao);

        }
        //Transferencia Conta Inexistente
        [Fact]
        public async Task TestaATransferenciaContaInexistente()
        {
            //Arrange
            var clienteFakeId = 500;
            
            var documentoDt = "1234567891789";
            var clienteDtMock = new Cliente() { Nome = "Antonio L Almeida", Documento = documentoDt, Conta = new Conta() { Saldo = 1500 } };
            
            var transferencia = 1500;

            await _context.Clientes.AddAsync(clienteDtMock);
            await _context.SaveChangesAsync();

            //Act
            var clienteDt = await _repositorioCliente.GetClienteByDocumentoAsync(documentoDt);
            var record = await Record.ExceptionAsync(async () => await _repository.TransferirAsync(clienteFakeId, clienteDt.Id, transferencia));

            //Assert
            Assert.NotNull(record);

        }
    }
}
