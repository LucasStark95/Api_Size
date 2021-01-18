using Microsoft.EntityFrameworkCore;
using Size.Core.Models;

namespace Size.Data.EFCore.Context
{
    public class Contexto : DbContext
    {
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        /// <summary>
        /// Definições das entidades para o mapameamento relacional.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

            base.OnModelCreating(modelBuilder);
        }
    }
}
