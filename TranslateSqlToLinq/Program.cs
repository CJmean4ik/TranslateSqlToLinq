using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TranslateSqlToLinq
{
    class Program
    {            
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            LinqExecute linqExecute = new LinqExecute(SourseBinder.GetSoursesItem);
            var list = linqExecute.GetAllItemsByFirstCondition()
                .Union(linqExecute.GetAllItemsBySecondCondition());
            stopwatch.Stop();

            Console.WriteLine($"Linq to Sql exucution = {stopwatch.ElapsedMilliseconds}");
            string heder = $"{"id",7} | {"item",7} | {"quantity",7} | {"cumalative",7} |";
            Console.WriteLine(heder);
            Console.WriteLine(new String('-',heder.Length));
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Id,7} | {item.item,7} | {item.quantity,8} | {item.Total,10} |");
            }


        }
       

    }
}
