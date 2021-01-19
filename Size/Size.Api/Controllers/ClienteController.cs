using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Size.Api.Controllers
{
    [Authorize]
    [Route("api/cliente")]
    [ApiController]
    public class ClienteController : Controller
    {
        public string Index()
        {
            return "Cliente";
        }
    }
}
