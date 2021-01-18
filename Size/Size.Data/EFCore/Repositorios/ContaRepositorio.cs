using Microsoft.EntityFrameworkCore;
using Size.Core.Models;
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
            return null;
        }

        public async Task<Conta> SaqueAsync(int idCliente, double valor)
        {
            return null;
        }

        public async Task<Conta> ExtratoAsync(int idCliente)
        {
            return null;
        }

        public async Task<bool> TransferirAsync(int idCliente, int idClienteDestino, double valor)
        {
            return false;
        }
    }
}
