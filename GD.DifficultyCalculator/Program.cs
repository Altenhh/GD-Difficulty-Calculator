using System;
using GD.Calculator.Online.Models;

namespace GD.Calculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int.TryParse(args[0], out int id);

            var level = OnlineLevel.GetLevel(id);
            Console.WriteLine(level.Title + " - " + level.ID + $" ({level.Level.ObjectCount} objs)");
        }
    }
}