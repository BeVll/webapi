using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        [HttpGet]
        public async Task<Table> Get(string leagueId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://www.fotmob.com/api/tltable?leagueId={leagueId}"),

            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();


                using JsonDocument doc = JsonDocument.Parse(body);
                JsonElement root = doc.RootElement;
                Table table = new Table(root[0]);
                return table;
            }

        }
    }
}
