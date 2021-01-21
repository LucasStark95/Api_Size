using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Size.Core.Models;
using Size.Data.EFCore.Repositorios;
using System.Threading.Tasks;

namespace Size.Api.Controllers
{
    [Authorize]
    [Route("api/cliente")]
    [ApiController]
    public class ClienteController : Controller
    {
        private readonly ClienteRepositorio _clienteRepositorio;

        public ClienteController(ClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<Cliente> CreateCliente([FromBody]Cliente cliente)
        {
            if (cliente.Conta == null)
                cliente.Conta = new Conta();

            var clienteDB = await  _clienteRepositorio.AdicionarAsync(cliente);

            return clienteDB;
        }
    }
}
