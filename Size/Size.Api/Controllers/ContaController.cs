using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Size.Core.Models;
using Size.Core.Request;
using Size.Data.EFCore.Repositorios;
using System.Threading.Tasks;

namespace Size.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class ContaController : Controller
    {
        private readonly ClienteRepositorio _clienteRepositorio;
        private readonly ContaRepositorio _contaRepositorio;
        public ContaController(ClienteRepositorio clienteRepositorio, ContaRepositorio contaRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _contaRepositorio = contaRepositorio;
        }

        [HttpPost]
        [Route("api/conta/deposito")]
        public async Task<IActionResult> DepositaContaAsync([FromBody] MovementRequest deposito)
        {
            var clienteDB = await GetClienteDbAsync(deposito.Documento);
            
            if(clienteDB != null)
            {
                var operacao = _contaRepositorio.DepositaAsync(clienteDB.Id, deposito.Valor);
                return Ok(new { Operacao = "Deposito ", Conta = operacao });
            }

            return BadRequest(new { result = "Não foi Possível realizar a operação."});
        }

        [HttpPost]
        [Route("api/conta/saque")]
        public async Task<IActionResult> SaqueContaAsync([FromBody] MovementRequest saque)
        {
            var clienteDB = await GetClienteDbAsync(saque.Documento);

            if (clienteDB != null)
            {
                var operacao = _contaRepositorio.SaqueAsync(clienteDB.Id, saque.Valor);
                return Ok(new { Operacao = "Saque", Conta = operacao });
            }

            return BadRequest(new { result = "Não foi Possível realizar a operação." });
        }

        [HttpPost]
        [Route("api/conta/transferencia")]
        public async Task<IActionResult> TransferenciaContaAsync([FromBody] TransactionRequest transacao)
        {
            var clienteRt = await GetClienteDbAsync(transacao.Remetente);
            var clienteDt = await GetClienteDbAsync(transacao.Destinatario);

            if (clienteRt != null && clienteDt != null)
            {
                var operacao = _contaRepositorio.TransferirAsync(clienteRt.Id, clienteDt.Id, transacao.Valor);
                return Ok(new { Operacao = "Transferencia", Result = operacao });
            }

            return BadRequest(new { result = "Não foi Possível realizar a operação." });
        }

        [HttpGet]
        [Route("api/conta/extrato/{documento}")]
        public async Task<IActionResult> ExtratoContaAsync(string documento)
        {
            var cliente = await GetClienteDbAsync(documento);
            if(cliente != null)
            {
                var extrato = await _contaRepositorio.ExtratoAsync(cliente.Id);
                return Ok(new { Operacao = "Transferencia", Extrato = extrato });
            }

            return BadRequest(new { result = "Não foi Possível realizar a operação." });
        }

        private async Task<Cliente> GetClienteDbAsync(string documento) 
            => await _clienteRepositorio.GetClienteByDocumentoAsync(documento);
    }
}
