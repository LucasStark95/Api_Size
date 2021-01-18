using Microsoft.EntityFrameworkCore;
using Size.Core.Models;
using System;
using System.Threading.Tasks;

namespace Size.Data.EFCore.Repositorios
{
    public class ContaRepositorio : RepositorioBase<Conta>
    {
        public ContaRepositorio(DbContext contexto) : base(contexto)
        {
        }

        public async Task<Conta> DepositaAsync(int idCliente, double valor)
        {

            ValidateValue(valor);

            var contaDb = await GetContaAsync(idCliente);

            ValidateConta(contaDb);

            contaDb.Saldo += valor;

            await AtualizarAsync(contaDb);

            return contaDb;
        }

        public async Task<Conta> SaqueAsync(int idCliente, double valor)
        {
            ValidateValue(valor);

            var contaDb = await GetContaAsync(idCliente);

            ValidateConta(contaDb);

            if (contaDb.Saldo - valor < 0) throw new Exception("Saldo Insuficiente para essa Operação.");

            contaDb.Saldo -= valor;

            await AtualizarAsync(contaDb);

            return contaDb;
        }

        public async Task<Conta> ExtratoAsync(int idCliente)
        {
            return await Buscar(c => c.ClienteId == idCliente).FirstOrDefaultAsync();
        }

        public async Task<bool> TransferirAsync(int idCliente, int idClienteDestino, double valor)
        {
            try
            {
                ValidateValue(valor);

                await SaqueAsync(idCliente, valor);
                await DepositaAsync(idClienteDestino, valor);

                return true;
            }
            catch
            {
                throw new Exception("Problema ao realizar essa operação."); ;
            }
        }

        private async Task<Conta> GetContaAsync(int idCliente)
        {
            return await Buscar(c => c.ClienteId == idCliente).FirstOrDefaultAsync();
        }

        private void ValidateValue(double valor)
        {
            if (valor < 0) throw new Exception("Valor não permitido.");
        }

        private void ValidateConta(Conta conta)
        {
            if (conta == null) throw new NullReferenceException("Conta não Localizada.");
        }
    }
}
