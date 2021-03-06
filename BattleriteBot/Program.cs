﻿using System;
using System.IO;
using System.Linq;

namespace BattleriteBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bot started!");
            string key;
            if (args.Any())
            {
                Console.WriteLine("Key found through args.");
                key = args[0];
            }
            else
            {
                Console.WriteLine("No args, finding key in file..");
                key = File.ReadAllText("../../../../key.txt");
            }
            Console.WriteLine("Key aquired!\nStarting up..");
            var com = new BotWrapper();
            var handler = new MainController();
            var comPros = new CommandProcessor(com, handler);
            com.Start(comPros, key);
        }
    }
}