using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimplyFly.API.Data;
using SimplyFly.API.DTOs;
using SimplyFly.API.Models;
using SimplyFly.API.Services.Implementations;

namespace SimplyFly.Tests
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private ApplicationDbContext _context;
        private PaymentService _paymentService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _paymentService = new PaymentService(_context);

            _context.Bookings.Add(new Booking
            {
                Id = 1,
                UserId = 101,
                FlightId = 1,
                SeatNumber = "A1",
                BookingDate = DateTime.Now,
                Status = "PendingPayment"
            });

            _context.Payments.Add(new Payment
            {
                Id = 1,
                BookingId = 1,
                Amount = 5000,
                PaymentDate = DateTime.Now,
                Status = "Pending"
            });

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }



        [Test]
        public void When_MakePayment_ValidBooking_ConfirmsBooking()
        {
            MakePaymentDto dto = new MakePaymentDto
            {
                BookingId = 1
            };

            ApiResponse<object> result = _paymentService.MakePayment(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Payment successful and booking confirmed"));
            Assert.That(_context.Bookings.First().Status, Is.EqualTo("Confirmed"));
            Assert.That(_context.Payments.First().Status, Is.EqualTo("Paid"));
        }

        [Test]
        public void When_MakePayment_InvalidBooking_ReturnsFailure()
        {
            MakePaymentDto dto = new MakePaymentDto
            {
                BookingId = 999
            };

            ApiResponse<object> result = _paymentService.MakePayment(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Booking not found"));
            Assert.That(_context.Bookings.First().Status, Is.EqualTo("PendingPayment"));
            Assert.That(_context.Payments.First().Status, Is.EqualTo("Pending"));
        }

        [Test]
        public void When_MakePayment_AlreadyPaid_ReturnsFailure()
        {
            _context.Payments.First().Status = "Paid";
            _context.SaveChanges();

            MakePaymentDto dto = new MakePaymentDto
            {
                BookingId = 1
            };

            ApiResponse<object> result = _paymentService.MakePayment(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Payment already completed"));
        }
    }
}
