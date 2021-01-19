using System;
using System.ComponentModel.DataAnnotations;

namespace Size.Core.Request
{
    public class TransactionRequest
    {
        [Required]
        public string Remetente { get; set; }
        [Required]
        public string Destinatario { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Valor { get; set; }
    }
}
