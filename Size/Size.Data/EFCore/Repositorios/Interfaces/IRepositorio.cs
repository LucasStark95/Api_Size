using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Size.Data.EFCore.Repositorios.Interfaces
{
    public interface IRepositorio<TEntity> where TEntity : class
    {
        Task<TEntity> AdicionarAsync(TEntity obj);

        Task<TEntity> AtualizarAsync(TEntity obj);

        IQueryable<TEntity> Buscar(Expression<Func<TEntity, bool>> clasulaWhere);

        IQueryable<TEntity> BuscarTodos();

        Task ExcluirAsync(Expression<Func<TEntity, bool>> clasulaWhere);
    }
}
