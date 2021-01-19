namespace Size.Core.Models
{
    public class ApiSettings
    {
        public string Secret { get; set; }
        public int TokenExpiry { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
