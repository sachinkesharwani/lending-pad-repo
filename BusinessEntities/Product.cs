using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities
{
    public class Product : IdObject
    {
        //public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public ProductCategory Category { get; private set; }
        public decimal Discount { get; private set; }

        public Product() { }
        //public Product(Guid id ,string name, decimal price, int stockQuantity, ProductCategory category)
        //{
        //    Id = id;
        //    Name = name;
        //    Price = price;
        //    StockQuantity = stockQuantity;
        //    Category = category;
        //}

        // Update the product name
        public void UpdateName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Name = name;
            }
        }

        // Update the product price
        public void UpdatePrice(decimal price)
        {
            if (price >= 0)
            {
                Price = price;
            }
        }

        // Add stock to the product
        public void AddStock(int stock)
        {
            if (stock > 0)
            {
                stock += stock;
            }
            else
            {
                throw new ArgumentException("Stock to add must be greater than zero");
            }
        }

        // Update the product category
        public void UpdateCategory(ProductCategory category)
        {
            Category = category;
        }

        public override string ToString()
        {
            return $"Product(Name={Name}, Price={Price}, Stock={Stock}, Category={Category})";
        }
        public decimal GetPriceAfterDiscount()
        {
            return Price * (1 - Discount / 100);
        }
    }
}

