using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibraryMembershipApp.Models;

namespace LibraryMembershipApp.Interfaces
{
    public interface IBookRepository
    {
        Book? GetBookById(int bookId);

        void MarkBookAsBorrowed(int bookId);
    }
}