using Microsoft.EntityFrameworkCore;
using Size.Core.Models;
using Size.Data.EFCore.Context;

namespace Size.Data.EFCore.Repositorios
{
    public class ClienteRepositorio : RepositorioBase<Cliente>
    {
        public ClienteRepositorio(Contexto contexto) : base(contexto)
        {
        }
    }
}
