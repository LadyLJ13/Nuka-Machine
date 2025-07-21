using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuka_Machine
{
    public class Drinks
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public ConsoleColor Color { get; set; }
        public Drinks(string name, int price, string description, ConsoleColor color, int stock = 10)
        {
            Name = name;
            Price = price;
            Description = description;
            Stock = stock;
            Color = color;
        }
    }
}
