using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Size.Api.Controllers
{
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
