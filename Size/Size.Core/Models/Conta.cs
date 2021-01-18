using Size.Core.Enums;
using Size.Core.Interfaces;

namespace Size.Core.Models
{
    public class Conta : IEntity, IConta
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Agencia { get; set; }
        public double Saldo { get; set; }
        public TipoEnum Tipo { get; set; }
    }
}
