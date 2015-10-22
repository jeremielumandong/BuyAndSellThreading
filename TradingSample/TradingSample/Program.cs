using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSample
{
    class Program
    {
        public static readonly List<string> Books =
            new List<string> { "C#", "Javascript", "Windows Workflow Foundation", "JAVA", "IIS", "WCF" };

        static void Main(string[] args)
        {
            InventoryProcessor controller = new InventoryProcessor();
            TimeSpan workDay = new TimeSpan(0, 0, 2);

            Task t1 = Task.Run(() => new SalesPerson("Bill Gates", ConsoleColor.Green).Work(controller, workDay));
            Task t2 = Task.Run(() => new SalesPerson("Mark Zuckerberg", ConsoleColor.Yellow).Work(controller, workDay));
            Task t3 = Task.Run(() => new SalesPerson("Elon Musk", ConsoleColor.Blue).Work(controller, workDay));
            Task t4 = Task.Run(() => new SalesPerson("Steve Jobs", ConsoleColor.Cyan).Work(controller, workDay));

            Task.WaitAll(t1, t2, t3, t4);
            controller.DisplayStatus();

            Console.ReadLine();
        }

    }

}
