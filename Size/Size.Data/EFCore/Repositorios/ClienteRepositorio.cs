using Microsoft.EntityFrameworkCore;
using Size.Core.Models;

namespace Size.Data.EFCore.Repositorios
{
    public class ClienteRepositorio : RepositorioBase<Cliente>
    {
        public ClienteRepositorio(DbContext contexto) : base(contexto)
        {
        }
    }
}
