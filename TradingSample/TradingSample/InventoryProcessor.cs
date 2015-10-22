using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace TradingSample
{
    public class InventoryProcessor
    {
        private readonly ConcurrentDictionary<string, int> inventory = new ConcurrentDictionary<string, int>();
        private int totalQuantityBought;
        private int totalQuantitySold;

        public void BuyStock(string item, int quantity)
        {
            this.inventory.AddOrUpdate(item, quantity, (key, oldValue) => oldValue + quantity);
            Interlocked.Add(ref this.totalQuantityBought, quantity);
        }

        public bool TrySellItem(string item)
        {
            var success = false;
            var newInventoryLevel = this.inventory.AddOrUpdate(item,
                itemName =>
                {
                    success = false;
                    return 0;
                },
                (itemName, oldValue) =>
                {
                    if (oldValue == 0)
                    {
                        success = false;
                        return 0;
                    }
                    success = true;
                    return oldValue - 1;
                });

            if (success)
                Interlocked.Increment(ref this.totalQuantitySold);

            return success;
        }

        public void DisplayStatus()
        {
            Console.ForegroundColor = ConsoleColor.White;
            var totalStock = this.inventory.Values.Sum();
            Console.WriteLine("\r\nBought = " + this.totalQuantityBought);
            Console.WriteLine("Sold   = " + this.totalQuantitySold);
            Console.WriteLine("Inventory  = " + totalStock);
            var error = totalStock + this.totalQuantitySold - this.totalQuantityBought;
            if (error == 0)
                Console.WriteLine("Inventory match");
            else
                Console.WriteLine("Error in inventory level: " + error);

            Console.WriteLine();
            Console.WriteLine("Inventory levels by item:");
            foreach (var itemName in Program.Books)
            {
                var stockLevel = this.inventory.GetOrAdd(itemName, 0);
                Console.WriteLine("{0,-30}: {1}", itemName, stockLevel);
            }
        }
    }
}