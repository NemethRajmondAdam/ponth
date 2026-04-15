using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ponth
{
    public class CartExtra
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // "Drink" or "Ingredient"
        public int Quantity { get; set; }
        public int Price { get; set; }  // per-unit price (mixing_price for drinks, price for ingredients)
    }

    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public List<CartExtra> Extras { get; set; } = new List<CartExtra>();

        public int ExtrasPrice
        {
            get
            {
                int total = 0;
                foreach (var extra in Extras)
                {
                    total += extra.Price * extra.Quantity;
                }
                return total;
            }
        }

        public int TotalItemPrice
        {
            get { return (Price + ExtrasPrice) * Quantity; }
        }
    }

    public class Cart
    {
        private Dictionary<int, CartItem> items = new Dictionary<int, CartItem>();
        private int nextUniqueId = -1;

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

        // Add a cocktail with extras — each call creates a unique entry
        public void AddItemWithExtras(int id, string name, int price, string type, List<CartExtra> extras)
        {
            if (extras == null || extras.Count == 0)
            {
                // No extras, use normal add
                AddItem(id, name, price, type);
                return;
            }

            int key = nextUniqueId--;
            items[key] = new CartItem
            {
                Id = id,
                Name = name,
                Price = price,
                Type = type,
                Quantity = 1,
                Extras = extras
            };
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
                total += item.TotalItemPrice;
            }
            return total;
        }

        // Get the dictionary key for a cart item (needed for remove operations on unique-keyed items)
        public int GetKey(CartItem item)
        {
            foreach (var kvp in items)
            {
                if (kvp.Value == item)
                    return kvp.Key;
            }
            return 0;
        }
    }
}
