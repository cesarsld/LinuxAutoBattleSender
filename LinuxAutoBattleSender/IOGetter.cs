using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace LinuxAutoBattleSender
{
    class IOGetter
    {
        private static readonly string baseAddressPath = "data/address_";
        private static readonly string baseAuthPath = "data/auth_token_";

        public static Dictionary<string, string> GetAddressAndAuthToken()
        {
            var addNum = Directory.GetFiles("data").Length / 2;
            var returnDict = new Dictionary<string, string>();
            for (int i = 1; i <= addNum; i++)
            {
                var add = File.ReadAllText(baseAddressPath + i.ToString() + ".txt"); 
                var auth = File.ReadAllText(baseAuthPath + i.ToString() + ".txt");
                returnDict.Add(add, auth);
            }
            return returnDict;
        }

    }
}
