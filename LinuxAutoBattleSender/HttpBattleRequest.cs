using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LinuxAutoBattleSender
{
    class HttpBattleRequest
    {
        public static async Task PostTeam(string address, string auth)
        {
            var teamJson = "";

            using (var cl = new HttpClient())
            {
                var data = await cl.GetAsync("https://api.axieinfinity.com/v1/battle/teams?address=" + address.ToLower() + "&offset=0&count=500&no_limit=1");
                teamJson = await data.Content.ReadAsStringAsync();
            }
            var json = JObject.Parse(teamJson);
            for (int i = 0; i < 3; i++)
            {
                foreach (var team in json["teams"])
                {
                    _ = SendTeam(JsonConvert.SerializeObject(new Team((string)team["teamId"])), auth);
                    await Task.Delay(500);

                }
            }
        }

        public static async Task SendTeam(string team, string auth)
        {
            var req = new HttpRequestMessage
            {
                RequestUri = new Uri("https://api.axieinfinity.com/v1/battle/battle/queue"),
                Method = HttpMethod.Post,
                Headers =
                    {
                        { HttpRequestHeader.Authorization.ToString(), "Bearer "  + auth},
                        { HttpRequestHeader.Accept.ToString(), "application/json"},
                        { HttpRequestHeader.ContentType.ToString(), "application/json"}
                    },
                Content = new StringContent(team, System.Text.Encoding.UTF8, "application/json")
            };
            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(req);
                var receive = await response.Content.ReadAsStringAsync();
                Console.WriteLine(receive);
            }
        }
    }
}
