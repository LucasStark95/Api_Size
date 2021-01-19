using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Size.Api.Controllers
{
    [Authorize]
    [Route("api/conta")]
    [ApiController]
    public class ContaController : Controller
    {
        public string Index()
        {
            return "Conta";
        }
    }
}
