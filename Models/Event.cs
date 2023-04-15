using System.Text.Json;

namespace webapi.Models
{
    public class Event
    {
        public int? EventId { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public string? Player { get; set; }
        public int? PlayerId { get; set; }
        public string? Card { get; set; }
        public bool? OwnGoal { get; set; }
        public bool? IsHome { get; set; }
        public string? PlayerAssist { get;set; }
        public int? AddedTime { get; set; }
        public string? PlayerSwap { get; set; }
        public int? PlayerSwapId { get; set; }
        public Event(JsonElement el)
        {
            if (el.TryGetProperty("eventId", out var top))
                this.EventId = el.GetProperty("eventId").GetInt32();
            this.Time = el.GetProperty("timeStr").ToString();
            this.Type = el.GetProperty("type").ToString();
            switch (el.GetProperty("type").ToString())
            {
                case "Goal":    
                    this.Player = el.GetProperty("player").GetProperty("name").ToString();
                    this.PlayerId = el.GetProperty("player").GetProperty("id").GetInt32();
        
                    if (el.GetProperty("ownGoal").ValueKind != JsonValueKind.Null)
                        this.OwnGoal = el.GetProperty("ownGoal").GetBoolean();
                    if (el.GetProperty("isHome").ValueKind != JsonValueKind.Null)
                        this.IsHome = el.GetProperty("isHome").GetBoolean();
                    this.PlayerAssist = el.GetProperty("assistStr").ToString().Replace("assist by", "");

                    break;
                case "Card":
                    this.Player = el.GetProperty("player").GetProperty("name").ToString();
                    this.PlayerId = el.GetProperty("player").GetProperty("id").GetInt32();

                    if (el.GetProperty("isHome").ValueKind != JsonValueKind.Null)
                        this.IsHome = el.GetProperty("isHome").GetBoolean();
                    
                    this.Card = el.GetProperty("card").ToString();
                    break;
                case "AddedTime":
                    this.AddedTime = el.GetProperty("minutesAddedInput").GetInt32();
                    break;
                case "Half":
                    if (el.GetProperty("halfStrShort").ToString() == "HT")
                        this.Type = "Half";
                    else if (el.GetProperty("halfStrShort").ToString() == "FT")
                        this.Type = "Full Time";
                    break;
                case "MissedPenalty":
                    this.Player = el.GetProperty("player").GetProperty("name").ToString();
                    this.PlayerId = el.GetProperty("player").GetProperty("id").GetInt32();

                    if (el.GetProperty("isHome").ValueKind != JsonValueKind.Null)
                        this.IsHome = el.GetProperty("isHome").GetBoolean();
                    break;
                case "Substitution":
                    this.Player = el.GetProperty("swap")[0].GetProperty("name").ToString();
                    this.PlayerId = Convert.ToInt32(el.GetProperty("swap")[0].GetProperty("id").ToString());
                    this.PlayerSwap = el.GetProperty("swap")[1].GetProperty("name").ToString();
                    this.PlayerSwapId = Convert.ToInt32(el.GetProperty("swap")[1].GetProperty("id").ToString());
                    if (el.GetProperty("isHome").ValueKind != JsonValueKind.Null)
                        this.IsHome = el.GetProperty("isHome").GetBoolean();
                    break;

            }
        }
    }
}
