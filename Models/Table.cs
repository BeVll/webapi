using System.Text.Json;

namespace webapi.Models
{
    public class Table
    {
        public string CountryCode { get; set; }
        public string LeagueId { get; set; }
        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }
        public List<Team> Teams { get; set; }
        public Table(JsonElement el)
        {
            if(el.GetProperty("data").TryGetProperty("ccode", out var c))
                this.CountryCode = el.GetProperty("data").GetProperty("ccode").ToString();
            this.LeagueId = el.GetProperty("data").GetProperty("leagueId").ToString();
            this.LeagueName = el.GetProperty("data").GetProperty("leagueName").ToString();
            this.LeagueLogo = $"https://images.fotmob.com/image_resources/logo/leaguelogo/dark/{this.LeagueId}.png";
            this.Teams = new List<Team>();
            for (int i = 0;i< el.GetProperty("data").GetProperty("table").GetProperty("all").GetArrayLength();i++)
            {
                this.Teams.Add(new Team(el.GetProperty("data").GetProperty("table").GetProperty("all")[i], true));
            }
        }
    }
}
