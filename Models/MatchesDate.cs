using System.Text.Json;

namespace webapi.Models
{
    public class MatchesDate
    {
        public int LeagueId { get; set; }
        public int LeaguePrimaryId { get; set; }
        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }
        public string CountryCode { get; set; }
        public List<MatchDetails> Matches { get; set; }
        public MatchesDate(JsonElement el)
        {
            this.LeagueId = el.GetProperty("id").GetInt32();
            this.LeaguePrimaryId = el.GetProperty("primaryId").GetInt32();
            this.LeagueName = el.GetProperty("name").ToString();
            this.CountryCode = el.GetProperty("ccode").ToString();
            this.Matches = new List<MatchDetails>();
            for(int i = 0; i < el.GetProperty("matches").GetArrayLength(); i++)
            {
                this.Matches.Add(new MatchDetails(el.GetProperty("matches")[i], this.LeagueName));

            }
            this.LeagueLogo = $"https://images.fotmob.com/image_resources/logo/leaguelogo/dark/{this.LeaguePrimaryId}.png";
        }
    }
}
