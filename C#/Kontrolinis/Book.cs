using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K1pvz6
{
    class Book
    {
        public string Destributor { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Book(string destributor, string title, int quantity, double price)
        {
            Destributor = destributor;
            Title = title;
            Quantity = quantity;
            Price = price;
        }
        public Book(string title)
        {
            Title = title;
            Quantity = 1;
        }
        public void SetHighestPrice(double price)
        {
            Price = price;
        }
        public static bool operator >= (Book bk1, double number)
        {
            if (bk1.Price >= number) return true;
            else return false;
        }
        public static bool operator <= (Book bk1, double number)
        {
            return !(bk1 >= number);
        }
        public override string ToString()
        {
            return String.Format("{0,-10} {1,-17} {2,8} {3,8}", Destributor, Title, Quantity, Price);
        }
    }
}
