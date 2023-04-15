using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        [HttpGet]
        public async Task<List<MatchesDate>> Get(string date)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://www.fotmob.com/api/matches?date={date}"),

            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();


                using JsonDocument doc = JsonDocument.Parse(body);
                JsonElement root = doc.RootElement;
                List<MatchesDate> matches = new List<MatchesDate>();
                for (int i = 0; i < root.GetProperty("leagues").GetArrayLength(); i++)
                {
                    matches.Add(new MatchesDate(root.GetProperty("leagues")[i]));
                }
                return matches;
            }

        }
    }
}
