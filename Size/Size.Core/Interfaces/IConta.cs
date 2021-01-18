using Size.Core.Enums;

namespace Size.Core.Interfaces
{
    public interface IConta
    {
        public int Numero { get; set; }
        public int Agencia { get; set; }
        public double Saldo { get; set; }
        public TipoEnum Tipo { get; set; }
    }
}
