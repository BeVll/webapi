using System.Text.Json;

namespace webapi.Models
{
    public class MatchDetails
    {
        public string Id { get; set; }
        public int LeagueId { get;set; }
        public string LeagueName { get; set;}
        public string? Round { get; set; }
        public string? Stadium { get; set; }
        public string? Referee { get; set; }
        public string? Season { get; set;}
        public string? CountryCode { get;set;}
        public Team HomeTeam { get; set;}
        public Team AwayTeam { get; set; }
        public bool isStarted { get; set; }
        public bool isFinished { get; set; }
        public bool isGoing { get; set; }
        public bool isCancelled { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public string HomeColor { get; set; }
        public string AwayColor { get; set; }
        public DateTime UtcTime { get; set; }
        public string? LiveTime { get; set; }
        public List<Event>? Goals { get; set; }
        public List<Event>? Events { get; set; }
        public List<Stat>? Stats { get; set; }
       
        List<string> statsName = new List<string> { "BallPossesion", "total_shots", "expected_goals", "accurate_passes", "fouls", "Offsides", "corners" };
        public MatchDetails(JsonElement el)
        {
            this.Id = el.GetProperty("general").GetProperty("matchId").ToString();
            this.LeagueId = el.GetProperty("general").GetProperty("parentLeagueId").GetInt32();
            this.LeagueName = el.GetProperty("general").GetProperty("parentLeagueName").ToString();
            this.Season = el.GetProperty("general").GetProperty("parentLeagueSeason").ToString();
            this.CountryCode = el.GetProperty("general").GetProperty("countryCode").ToString();

            if(el.GetProperty("content").GetProperty("matchFacts").GetProperty("infoBox").GetProperty("Tournament").TryGetProperty("round", out JsonElement round))
                this.Round = round.ToString();
            if (el.GetProperty("content").GetProperty("matchFacts").GetProperty("infoBox").GetProperty("Stadium").TryGetProperty("name", out JsonElement stadium))
                this.Stadium = stadium.ToString();
            if (el.GetProperty("content").GetProperty("matchFacts").GetProperty("infoBox").GetProperty("Referee").TryGetProperty("text", out JsonElement referee))
                this.Referee = referee.ToString();

            this.HomeTeam = new Team(el.GetProperty("header").GetProperty("teams")[0]);
            this.AwayTeam = new Team(el.GetProperty("header").GetProperty("teams")[1]);
            this.isStarted = el.GetProperty("general").GetProperty("started").GetBoolean();
            this.isFinished = el.GetProperty("general").GetProperty("finished").GetBoolean();
            this.isGoing = el.GetProperty("ongoing").GetBoolean();
            this.isCancelled = el.GetProperty("header").GetProperty("status").GetProperty("cancelled").GetBoolean();
            this.HomeScore = el.GetProperty("header").GetProperty("teams")[0].GetProperty("score").GetInt32();
            this.AwayScore = el.GetProperty("header").GetProperty("teams")[1].GetProperty("score").GetInt32();
            this.HomeColor = el.GetProperty("content").GetProperty("stats").GetProperty("teamColors").GetProperty("homeColors").GetProperty("color").ToString();
            this.AwayColor = el.GetProperty("content").GetProperty("stats").GetProperty("teamColors").GetProperty("awayColors").GetProperty("colorAway").ToString();
            this.UtcTime = el.GetProperty("header").GetProperty("status").GetProperty("utcTime").GetDateTime();
            if(this.isStarted && this.isGoing)
                this.LiveTime = el.GetProperty("header").GetProperty("status").GetProperty("liveTime").GetProperty("short").ToString();
            else if (this.isFinished)
                this.LiveTime = "Finished";

            if (this.isStarted)
            {
                this.Goals = new List<Event>();
                List<JsonElement> list = el.GetProperty("content").GetProperty("matchFacts").GetProperty("events").GetProperty("events").EnumerateArray().ToList().Where(s => s.GetProperty("type").ToString() == "Goal").ToList();
                foreach (JsonElement item in list)
                {
                   this.Goals.Add(new Event(item));
                }

                this.Events = new List<Event>();
                List<JsonElement> events = el.GetProperty("content").GetProperty("matchFacts").GetProperty("events").GetProperty("events").EnumerateArray().ToList();
                foreach (JsonElement item in events)
                {
                    this.Events.Add(new Event(item));
                }

                this.Stats = new List<Stat>();
                List<JsonElement> stats = el.GetProperty("content").GetProperty("stats").GetProperty("stats").EnumerateArray().ToList();
                foreach (JsonElement item in stats)
                {
                    List<JsonElement> stats2 = item.GetProperty("stats").EnumerateArray().ToList();
                    foreach (JsonElement item1 in stats2)
                    {
                        if (statsName.Contains(item1.GetProperty("key").ToString()))
                        {
                            if (this.Stats.Where(s => s.Key == item1.GetProperty("key").ToString()).FirstOrDefault() == null)
                                this.Stats.Add(new Stat(item1));
                        }
                            
                    }
                }

            }

        }

        public MatchDetails(JsonElement el, string leagueName)
        {
            this.Id = el.GetProperty("id").ToString();
            this.LeagueId = el.GetProperty("leagueId").GetInt32();
            this.LeagueName = leagueName;
            this.UtcTime = el.GetProperty("status").GetProperty("utcTime").GetDateTime();
            this.HomeTeam = new Team(el.GetProperty("home"), true);
            this.AwayTeam = new Team(el.GetProperty("away"), true);
            if (el.GetProperty("home").GetProperty("score").ValueKind != JsonValueKind.Null)
                this.HomeScore = el.GetProperty("home").GetProperty("score").GetInt32();
            if (el.GetProperty("away").GetProperty("score").ValueKind != JsonValueKind.Null)
                this.AwayScore = el.GetProperty("away").GetProperty("score").GetInt32();
         
            this.isStarted = el.GetProperty("status").GetProperty("started").GetBoolean();
            this.isFinished = el.GetProperty("status").GetProperty("finished").GetBoolean();
            if (el.GetProperty("status").TryGetProperty("ongoing", out var d))
                this.isGoing = el.GetProperty("status").GetProperty("ongoing").GetBoolean();
            this.isCancelled = el.GetProperty("status").GetProperty("cancelled").GetBoolean();
            if (this.isStarted && this.isGoing)
            {
                if (el.GetProperty("status").TryGetProperty("liveTime", out var t))
                {
                    if (el.GetProperty("status").GetProperty("liveTime").TryGetProperty("short", out var f))
                        this.LiveTime = el.GetProperty("status").GetProperty("liveTime").GetProperty("short").ToString();
                }
                    
            }
                
            else if (this.isFinished)
                this.LiveTime = "FT";
        }
    }
}
