using Size.Core.Interfaces;

namespace Size.Core.Models
{
    public class Cliente : IEntity, ICliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Documento { get; set; }
    }
}
