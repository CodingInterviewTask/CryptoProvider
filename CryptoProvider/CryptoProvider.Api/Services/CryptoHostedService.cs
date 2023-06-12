using CryptoProvider.Api.Controllers;
using CryptoProvider.Api.WebSocket;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProvider.Api.Services
{
    public class CryptoHostedService : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private HttpClient _httpClient = new HttpClient();
        private static IHubContext<CryptoHub> _hub;
        public static List<CryptoData> CryptoDataStore { get; private set; } = new();

        public CryptoHostedService(IHubContext<CryptoHub> hub)
        {
            _hub = hub;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(GetCryptoData, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void GetCryptoData(object? state)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://data.binance.com/api/v3/ticker/24hr");
            var response = _httpClient.Send(request);
            var data = response.Content.ReadFromJsonAsync<List<CryptoData>>().Result;
            CryptoDataStore = data;

            var symbol = CryptoController.SetectedSymbolStore;

            var result = data?.Where(x => x.Symbol == symbol).FirstOrDefault();

            _hub.Clients?.All?.SendAsync("CryptoDataResponse", result);
        }
    }
}
