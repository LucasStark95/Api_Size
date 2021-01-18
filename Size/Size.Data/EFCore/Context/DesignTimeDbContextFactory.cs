using Microsoft.EntityFrameworkCore;

namespace Size.Data.EFCore.Context
{
    public class DesignTimeDbContextFactory : DesignTimeDbContextFactoryBase<Contexto>
    {
        protected override Contexto CreateNewInstance(
            DbContextOptions<Contexto> options)
        {
            return new Contexto(options);
        }
    }
}
