using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace K1pvz6
{
    class Program
    {
        static void Main(string[] args)
        {
            BookStore boughtBooks = new BookStore();
            BookStore soldBooks = new BookStore();
            ReadData(boughtBooks, soldBooks);
            AddHighestPriceOnTitle(boughtBooks, soldBooks);
            double debt = 0;
            debt = boughtBooks.Debt() - soldBooks.Debt();
            WriteData(soldBooks, debt);
        }
        public static void ReadData(BookStore boughtBooks, BookStore soldBooks)
        {
            using (StreamReader sr = new StreamReader("Knyga.txt")) 
            {
                string line = null;
                while (null != (line = sr.ReadLine()))
                {
                    string[] values = line.Split(';');
                    string destributor = values[0];
                    string title = values[1];
                    int quantity = Convert.ToInt32(values[2]);
                    double price = Convert.ToDouble(values[3]);
                    Book book = new Book(destributor, title, quantity, price);
                    boughtBooks.AddBook(book);
                }
            }
            using (StreamReader sr = new StreamReader("Parduota.txt"))
            {
                string line = null;
                while (null != (line = sr.ReadLine()))
                {
                    string title = line;
                    Book book = new Book(title);
                    soldBooks.AddBook(book);
                }
            }
        }
        public static double FindHighestPrice(Book targetBook, BookStore boughtBooks)
        {
            double max = -1;
            for (int i = 0; i < boughtBooks.bookCount; i++)
            {
                if (targetBook.Title == boughtBooks.Book[i].Title)
                {
                    if (boughtBooks.Book[i] >= max)
                    {
                        max = boughtBooks.Book[i].Price;
                    }
                }
            }
            return max;
        }
        public static void AddHighestPriceOnTitle(BookStore boughtBooks, BookStore soldBooks)
        {
            for(int i = 0; i< soldBooks.bookCount; i++)
            {
                soldBooks.Book[i].SetHighestPrice(FindHighestPrice(soldBooks.Book[i], boughtBooks));
            }
        }
        public static void WriteData(BookStore soldBooks, double debt)
        {
            using (StreamWriter sw = new StreamWriter("Rezultatai.txt"))
            {
                string topic = String.Format("{0,-10} {1,-17} {2,8} {3,8}", "Leidejas", "Pavadinimas", "Kiekis", "Kaina");
                sw.WriteLine(new string('-', 50));
                sw.WriteLine(topic);
                sw.WriteLine(new string('-', 50));
                for(int i = 0; i < soldBooks.bookCount; i++)
                {
                    sw.WriteLine(soldBooks.Book[i]);
                }
                sw.WriteLine();
                sw.WriteLine(debt);
            }
        }
    }
}
