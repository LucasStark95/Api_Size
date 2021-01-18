using Microsoft.Extensions.Configuration;

namespace Size.Data.EFCore.Helpers
{
    /// <summary>
    /// Connection Helper
    /// Classe Auxiliar para a conexão do banco.
    /// </summary>
    public class ConnectionHelper
    {
        /// <summary>
        /// Busca em arquivo do tipo JSon a string de conexão com o banco.
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString() => new ConfigurationBuilder()
            .AddJsonFile("settings.json")
            .Build().GetConnectionString("Size");
    }
}
