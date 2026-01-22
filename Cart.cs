using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ponth
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }

    public class Cart
    {
        private Dictionary<int, CartItem> items = new Dictionary<int, CartItem>();

        public IReadOnlyCollection<CartItem> Items => items.Values;

        public void AddItem(int id, string name, int price, string type)
        {
            if (items.ContainsKey(id))
            {
                items[id].Quantity++;
            }
            else
            {
                items[id] = new CartItem
                {
                    Id = id,
                    Name = name,
                    Price = price,
                    Type = type,
                    Quantity = 1
                };
            }
        }

        public void RemoveOne(int id)
        {
            if (items.ContainsKey(id))
            {
                items[id].Quantity--;
                if (items[id].Quantity <= 0)
                    items.Remove(id);
            }
        }

        public void RemoveItem(int id)
        {
            if (items.ContainsKey(id))
            {
                items.Remove(id);
            }
        }

        public void Clear()
        {
            items.Clear();
        }

        public int TotalPrice()
        {
            int total = 0;
            foreach (var item in items.Values)
            {
                total += item.Price * item.Quantity;
            }
            return total;
        }
    }
}
