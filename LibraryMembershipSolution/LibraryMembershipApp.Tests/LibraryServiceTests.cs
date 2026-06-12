using Moq;
using NUnit.Framework;

using LibraryMembershipApp.Interfaces;
using LibraryMembershipApp.Models;
using LibraryMembershipApp.Services;

namespace LibraryMembershipApp.Tests
{
    [TestFixture]
    public class LibraryServiceTests
    {
        private Mock<IMemberRepository> _memberRepositoryMock;
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<INotificationService> _notificationServiceMock;


    private LibraryService _libraryService;

        [SetUp]
        public void Setup()
        {
            _memberRepositoryMock = new Mock<IMemberRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _notificationServiceMock = new Mock<INotificationService>();

            _libraryService = new LibraryService(
                _memberRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _notificationServiceMock.Object);
        }

        [Test]
        public void BorrowBook_WhenAllConditionsAreValid_ShouldReturnSuccessMessage()
        {
            
            var member = new Member
            {
                MemberId = 1,
                MemberName = "Ravi",
                Email = "ravi@gmail.com",
                IsActive = true,
                BorrowedBookCount = 1
            };

            var book = new Book
            {
                BookId = 101,
                BookTitle = "Clean Code",
                AuthorName = "Robert Martin",
                IsAvailable = true
            };

            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns(member);
            _bookRepositoryMock.Setup(x => x.GetBookById(101)).Returns(book);

            
            var result = _libraryService.BorrowBook(1, 101);

            
            Assert.That(result, Is.EqualTo("Book borrowed successfully"));

            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(101), Times.Once);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(1), Times.Once);

            _notificationServiceMock.Verify(
                x => x.SendBorrowNotification("ravi@gmail.com", "Clean Code"),
                Times.Once);
        }

        [Test]
        public void BorrowBook_WhenMemberDoesNotExist_ShouldReturnMemberNotFound()
        {
            
            _memberRepositoryMock.Setup(x => x.GetMemberById(1))
                                 .Returns((Member?)null);

            
            var result = _libraryService.BorrowBook(1, 101);

            
            Assert.That(result, Is.EqualTo("Member not found"));
        }

        [Test]
        public void BorrowBook_WhenMemberIsInactive_ShouldReturnMemberIsNotActive()
        {
            
            var member = new Member
            {
                MemberId = 1,
                IsActive = false
            };

            _memberRepositoryMock.Setup(x => x.GetMemberById(1))
                                 .Returns(member);

           
            var result = _libraryService.BorrowBook(1, 101);

            
            Assert.That(result, Is.EqualTo("Member is not active"));
        }

        [Test]
        public void BorrowBook_WhenBookDoesNotExist_ShouldReturnBookNotFound()
        {
            
            var member = new Member
            {
                MemberId = 1,
                IsActive = true,
                BorrowedBookCount = 1
            };

            _memberRepositoryMock.Setup(x => x.GetMemberById(1))
                                 .Returns(member);

            _bookRepositoryMock.Setup(x => x.GetBookById(101))
                               .Returns((Book?)null);

            
            var result = _libraryService.BorrowBook(1, 101);

            
            Assert.That(result, Is.EqualTo("Book not found"));
        }

        [Test]
        public void BorrowBook_WhenBookIsNotAvailable_ShouldReturnBookIsNotAvailable()
        {
            
            var member = new Member
            {
                MemberId = 1,
                IsActive = true,
                BorrowedBookCount = 1
            };

            var book = new Book
            {
                BookId = 101,
                IsAvailable = false
            };

            _memberRepositoryMock.Setup(x => x.GetMemberById(1))
                                 .Returns(member);

            _bookRepositoryMock.Setup(x => x.GetBookById(101))
                               .Returns(book);

            
            var result = _libraryService.BorrowBook(1, 101);

            
            Assert.That(result, Is.EqualTo("Book is not available"));
        }

        [Test]
        public void BorrowBook_WhenBorrowingLimitReached_ShouldReturnBorrowingLimitReached()
        {
            
            var member = new Member
            {
                MemberId = 1,
                IsActive = true,
                BorrowedBookCount = 3
            };

            var book = new Book
            {
                BookId = 101,
                IsAvailable = true
            };

            _memberRepositoryMock.Setup(x => x.GetMemberById(1))
                                 .Returns(member);

            _bookRepositoryMock.Setup(x => x.GetBookById(101))
                               .Returns(book);

            
            var result = _libraryService.BorrowBook(1, 101);

            
            Assert.That(result, Is.EqualTo("Borrowing limit reached"));
        }

        [Test]
        public void BorrowBook_WhenMemberIdIsInvalid_ShouldReturnInvalidMemberId()
        {
            
            var result = _libraryService.BorrowBook(0, 101);

            
            Assert.That(result, Is.EqualTo("Invalid member id"));
        }

        [Test]
        public void BorrowBook_WhenBookIdIsInvalid_ShouldReturnInvalidBookId()
        {
            
            var result = _libraryService.BorrowBook(1, 0);

            
            Assert.That(result, Is.EqualTo("Invalid book id"));
        }
    }


}
