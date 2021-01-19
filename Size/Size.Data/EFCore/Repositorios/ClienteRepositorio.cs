using Microsoft.EntityFrameworkCore;
using Size.Core.Models;
using Size.Data.EFCore.Context;
using System.Threading.Tasks;

namespace Size.Data.EFCore.Repositorios
{
    public class ClienteRepositorio : RepositorioBase<Cliente>
    {
        public ClienteRepositorio(Contexto contexto) : base(contexto)
        {
        }

        public async Task<Cliente> GetClienteByLogin(Login login) => await Buscar(c => c.Nome == login.Nome 
                                                                && c.Documento == login.Documento)
                                                                .FirstOrDefaultAsync();
    }
}
