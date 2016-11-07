using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K1pvz6
{
    class BookStore
    {
        const int maxNumberOfBooks = 50;
        public Book[] Book { get; set; }
        public int bookCount { get; set; }

        public BookStore()
        {
            Book = new Book[maxNumberOfBooks];
        }
        public void AddBook(Book book)
        {
            Book[bookCount++] = book;
        }
        public double Debt()
        {
            double debt = 0;
            for(int i = 0; i < bookCount; i++)
            {
                debt += Book[i].Price * Book[i].Quantity;
            }
            return debt;
        }
    }
}
