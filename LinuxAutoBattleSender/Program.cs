using System;
using System.Collections.Generic;
using System.Linq;

namespace LinuxAutoBattleSender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            LoopHandler.SendTeamsLoop().GetAwaiter().GetResult();
        }
    }
}
