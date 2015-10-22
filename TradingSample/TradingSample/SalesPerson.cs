using System;
using System.Threading;

namespace TradingSample
{
    public class SalesPerson
    {
        private readonly ConsoleColor currentConsoleColor;

        public SalesPerson(string id, ConsoleColor color)
        {
            this.Name = id;
            this.currentConsoleColor = color;
        }

        public string Name { get; }

        public void Work(InventoryProcessor inventoryProcessor, TimeSpan workDay)
        {
            var rand = new Random(this.Name.GetHashCode());
            var start = DateTime.Now;
            while (DateTime.Now - start < workDay)
            {
                Thread.Sleep(rand.Next(100));
                var buy = (rand.Next(6) == 0);
                var itemName = Program.Books[rand.Next(Program.Books.Count)];
                if (buy)
                {
                    var quantity = rand.Next(9) + 1;
                    inventoryProcessor.BuyStock(itemName, quantity);
                    this.DisplayPurchase(itemName, quantity);
                }
                else
                {
                    var success = inventoryProcessor.TrySellItem(itemName);
                    this.DisplaySaleAttempt(success, itemName);
                }
            }
            Console.WriteLine("SalesPerson {0} signing off", this.Name);
        }

        public void DisplayPurchase(string itemName, int quantity)
        {
            Console.ForegroundColor = this.currentConsoleColor;
            Console.WriteLine("Thread {0}: {1} bought {2} of {3}", Thread.CurrentThread.ManagedThreadId, this.Name,
                quantity, itemName);
        }

        public void DisplaySaleAttempt(bool success, string itemName)
        {
            Console.ForegroundColor = this.currentConsoleColor;
            var threadId = Thread.CurrentThread.ManagedThreadId;
            if (success)
                Console.WriteLine("Thread {0}: {1} sold {2}", threadId, this.Name, itemName);
            else
                Console.WriteLine("Thread {0}: {1}: Out of stock of {2}", threadId, this.Name, itemName);
        }
    }
}