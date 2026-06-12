using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using LibraryMembershipApp.Interfaces;
using LibraryMembershipApp.Services;

namespace LibraryMembershipApp.Services
{
    public class LibraryService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IBookRepository _bookRepository;
        private readonly INotificationService _notificationService;

        public LibraryService(
            IMemberRepository memberRepository,
            IBookRepository bookRepository,
            INotificationService notificationService)
        {
            _memberRepository = memberRepository;
            _bookRepository = bookRepository;
            _notificationService = notificationService;
        }

        public string BorrowBook(int memberId, int bookId)
        {
            
            if (memberId <= 0)
            {
                return "Invalid member id";
            }

            
            if (bookId <= 0)
            {
                return "Invalid book id";
            }

            
            var member = _memberRepository.GetMemberById(memberId);

            if (member == null)
            {
                return "Member not found";
            }

            if (!member.IsActive)
            {
                return "Member is not active";
            }

            
            var book = _bookRepository.GetBookById(bookId);

            if (book == null)
            {
                return "Book not found";
            }

            if (!book.IsAvailable)
            {
                return "Book is not available";
            }

            
            if (member.BorrowedBookCount >= 3)
            {
                return "Borrowing limit reached";
            }

            
            _bookRepository.MarkBookAsBorrowed(bookId);

            _memberRepository.UpdateBorrowedBookCount(memberId);

            _notificationService.SendBorrowNotification(
                member.Email,
                book.BookTitle);

            return "Book borrowed successfully";
        }
    }
}