
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace LinuxAutoBattleSender
{
    class LoopHandler
    {
        public static int lastUnixTimeCheck = -1;
        public static readonly int unixTimeBetweenUpdates = 43200/2;

        public static async Task SendTeamsLoop()
        {
            while (true)
            {
                int unixTime = Convert.ToInt32(((DateTimeOffset)(DateTime.UtcNow)).ToUnixTimeSeconds());
                if (lastUnixTimeCheck == -1) UpdateUnixLastCheck();
                if (lastUnixTimeCheck == 0)
                {
                    SetupFirstTime(unixTime);
                    UpdateUnixLastCheck();
                }
                Console.Clear();
                Console.WriteLine($"Time before next battles :  {unixTimeBetweenUpdates - (unixTime - lastUnixTimeCheck)} seconds");
                if (unixTime - lastUnixTimeCheck >= unixTimeBetweenUpdates)
                {
                    lastUnixTimeCheck = unixTime;
                    var dict = IOGetter.GetAddressAndAuthToken();
                    foreach (var add in dict.Keys)
                    {
                        Console.WriteLine("Sending teams from : " + add);
                        await HttpBattleRequest.PostTeam(add, dict[add]);
                    }
                    File.WriteAllText("time/LastTimeCheck.txt", lastUnixTimeCheck.ToString());
                }
                await Task.Delay(60000);
            }
        }

        public static void UpdateUnixLastCheck()
        {
            lastUnixTimeCheck = Convert.ToInt32(File.ReadAllText("time/LastTimeCheck.txt"));
        }

        public static void SetupFirstTime(int time)
        {
            File.WriteAllText("time/LastTimeCheck.txt", time.ToString());
        }
    }
}