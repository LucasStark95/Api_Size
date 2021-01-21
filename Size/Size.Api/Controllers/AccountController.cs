using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Size.Api.Helpers;
using Size.Core.Models;
using Size.Core.Request;
using Size.Data.EFCore.Repositorios;
using System.Threading.Tasks;

namespace Size.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ApiSettings _apiSettings;
        private readonly ClienteRepositorio _clienteRepositorio;

        public AccountController(IOptions<ApiSettings> settings, ClienteRepositorio clienteRepositorio)
        {
            _apiSettings = settings.Value;
            _clienteRepositorio = clienteRepositorio;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody]Login login)
        {
            if (login == null) return Unauthorized();
            var validUser = await Authenticate(login);

            string tokenString;
            if (validUser)
            {
                tokenString = TokenHandler.BuildJWTToken(_apiSettings);
            }
            else
            {
                return Unauthorized();
            }

            return Ok(new { Token = tokenString });
        }

        private async Task<bool> Authenticate(Login login)
        {
            var cliente = await _clienteRepositorio.GetClienteByLogin(login);
            return cliente!= null ? true : false;
        }


    }
}
