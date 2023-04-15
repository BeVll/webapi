using System.Text.Json;

namespace webapi.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int? PlayedGames { get; set; }
        public int? Wins { get; set; }
        public int? Draws { get; set; }
        public int? Losses { get; set; }
        public int? Points { get; set; }
        public int? Gd { get; set; }
        public Team(JsonElement el)
        {
            this.Id = el.GetProperty("id").GetInt32();
            this.Name = el.GetProperty("name").ToString();
            this.ImageUrl = el.GetProperty("imageUrl").ToString();
        }
        public Team(JsonElement el, bool withoutImage)
        {
            this.Id = el.GetProperty("id").GetInt32();
            if (el.TryGetProperty("shortName", out var m))
                this.Name = m.ToString();
            else
                this.Name = el.GetProperty("name").ToString();

            this.ImageUrl = $"https://images.fotmob.com/image_resources/logo/teamlogo/{this.Id}_small.png";
            if(el.TryGetProperty("played", out var p))
                this.PlayedGames = p.GetInt32();
            if (el.TryGetProperty("wins", out var w))
                this.Wins = w.GetInt32();
            if (el.TryGetProperty("draws", out var d))
                this.Draws = d.GetInt32();
            if (el.TryGetProperty("losses", out var l))
                this.Losses = l.GetInt32();
            if (el.TryGetProperty("pts", out var pt))
                this.Points = pt.GetInt32();
            if (el.TryGetProperty("goalConDiff", out var gd))
                this.Gd = gd.GetInt32();
        }
    }
}
