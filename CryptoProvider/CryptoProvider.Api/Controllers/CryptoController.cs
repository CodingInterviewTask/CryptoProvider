using CryptoProvider.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace CryptoProvider.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CryptoController : ControllerBase
    {
        public static string SetectedSymbolStore = "BTCUSDT";

        [HttpPost(nameof(ChangeSymbol))]
        public IActionResult ChangeSymbol(ChangeSymbolRequest request)
        {
            SetectedSymbolStore = request.Symbol;
            var data = CryptoHostedService.CryptoDataStore.Where(x => x.Symbol == request.Symbol).FirstOrDefault();
            return Ok(data);
        }
    }

    public class CryptoData
    {
        public string Symbol { get; set; }
        public float BidPrice { get; set; }
        public float AskPrice { get; set; }
        public double LastId { get; set; }
    }

    public class ChangeSymbolRequest
    {
        public string Symbol { get; set; }
    }
}
