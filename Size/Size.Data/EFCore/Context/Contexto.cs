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

            modelBuilder.HasSequence<int>("AccountGenerate")
                .StartsAt(1000)
                .IncrementsBy(1);

            modelBuilder.Entity<Conta>()
                        .Property(c => c.Numero)
                        .HasDefaultValueSql("nextval('\"AccountGenerate\"')");

            modelBuilder.Entity<Conta>()
                    .Property(c => c.Agencia)
                    .HasDefaultValue(0);

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Conta)
                .WithOne(c => c.Cliente)
                .HasForeignKey<Conta>(c => c.ClienteId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
