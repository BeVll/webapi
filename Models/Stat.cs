using System.Text.Json;
using System.Text.RegularExpressions;

namespace webapi.Models
{
    public class Stat
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string ValueHome { get; set; }
        public string ValueAway { get; set; }
        public string PercentHome { get; set; }
        public string PercentAway { get; set; }
        public Stat(JsonElement el)
        {
            this.Key = el.GetProperty("key").ToString();
            this.ValueHome = el.GetProperty("stats")[0].ToString();
            this.ValueAway = el.GetProperty("stats")[1].ToString();
            string body = File.ReadAllText("Configs/Renamer.json");
            JsonDocument doc = JsonDocument.Parse(body);
            JsonElement root = doc.RootElement;
            this.Name = root.GetProperty(this.Key).ToString();
            if (this.Key == "BallPossesion")
                this.Key += "%";
            string value1 = this.ValueHome.Replace('.', ',');
            string value2 = this.ValueAway.Replace('.', ',');
            if(!double.TryParse(value1, out double val1))
            {
                Regex regex = new Regex(@"(\d)+");
                MatchCollection matches = regex.Matches(value1);
                value1 = matches[0].ToString();
                
            }
            if(!double.TryParse (value2, out double val2))
            {
                Regex regex2 = new Regex(@"(\d)+");
                MatchCollection matches2 = regex2.Matches(value2);
                value2 = matches2[0].ToString();
            }
            
            this.PercentHome = (Convert.ToDouble(value1) / Convert.ToDouble(value2)).ToString();
            this.PercentAway = (Convert.ToDouble(value2) / Convert.ToDouble(value1)).ToString();

            this.PercentHome = this.PercentHome.Replace(",", ".");
            this.PercentAway = this.PercentAway.Replace(",", ".");
        }
    }
}
