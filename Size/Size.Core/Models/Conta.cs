using Size.Core.Enums;
using Size.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Size.Core.Models
{
    public class Conta : IEntity, IConta
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Agencia { get; set; }
        [Range(0, int.MaxValue)]
        public double Saldo { get; set; }
        public TipoEnum Tipo { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
