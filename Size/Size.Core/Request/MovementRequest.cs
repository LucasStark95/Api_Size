using System.ComponentModel.DataAnnotations;

namespace Size.Core.Request
{
    public class MovementRequest
    {
        [Required]
        public string Documento { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Valor { get; set; }
    }
}
