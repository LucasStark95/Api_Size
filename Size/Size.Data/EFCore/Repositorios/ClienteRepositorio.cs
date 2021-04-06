using Microsoft.EntityFrameworkCore;
using Size.Core.Models;
using Size.Core.Request;
using Size.Data.EFCore.Context;
using Size.Data.EFCore.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Size.Data.EFCore.Repositorios
{
    public class ClienteRepositorio : RepositorioBase<Cliente>
    {
        public ClienteRepositorio(Contexto contexto) : base(contexto)
        {
        }

        public async Task<Cliente> GetClienteByLogin(Login login) 
            => await Buscar(c => c.Nome == login.Nome 
                && c.Documento == login.Documento)
               .FirstOrDefaultAsync();
        public async Task<Cliente> GetClienteByDocumentoAsync(string documento)
        {
            return await Buscar(c => c.Documento.Contains(documento))
               .FirstOrDefaultAsync();
        }

        public IEnumerable<Cliente> GetTodosClientes()
        {
            return BuscarTodos().OrderByDynamic("Nome", true);
        }
    }
}
