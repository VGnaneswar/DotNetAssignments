using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimplyFly.API.Data;
using SimplyFly.API.DTOs;
using SimplyFly.API.Models;
using SimplyFly.API.Services.Implementations;
using System.Security.Claims;

namespace SimplyFly.Tests
{
    [TestFixture]
    public class BookingServiceTests
    {
        private ApplicationDbContext _context;
        private BookingService _bookingService;
        private ClaimsPrincipal _user;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _bookingService = new BookingService(_context);

            _user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "101"),
                new Claim(ClaimTypes.Role, "Passenger")
            }, "TestAuth"));

            _context.Flights.Add(new Flight
            {
                Id = 1,
                FlightNumber = "SF101",
                FlightName = "SimplyFly",
                Origin = "Hyderabad",
                Destination = "Bangalore",
                DepartureTime = DateTime.Now.AddHours(2),
                ArrivalTime = DateTime.Now.AddHours(4),
                Fare = 5000,
                TotalSeats = 10,
                AvailableSeats = 10
            });

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void When_BookFlight_ValidDetails_CreatesBookingAndPayment()
        {
            BookFlightDto dto = new BookFlightDto
            {
                FlightId = 1,
                SeatNumber = "A1"
            };

            ApiResponse<object> result = _bookingService.BookFlight(dto, _user);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Booking created. Please complete payment to confirm booking."));
            Assert.That(_context.Bookings.Count(), Is.EqualTo(1));
            Assert.That(_context.Payments.Count(), Is.EqualTo(1));
            Assert.That(_context.Bookings.First().Status, Is.EqualTo("PendingPayment"));
            Assert.That(_context.Payments.First().Status, Is.EqualTo("Pending"));
            Assert.That(_context.Flights.First().AvailableSeats, Is.EqualTo(9));
        }

        [Test]
        public void When_BookFlight_InvalidFlight_ReturnsFailure()
        {
            BookFlightDto dto = new BookFlightDto
            {
                FlightId = 999,
                SeatNumber = "A1"
            };

            ApiResponse<object> result = _bookingService.BookFlight(dto, _user);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Flight not found"));
            Assert.That(_context.Bookings.Count(), Is.EqualTo(0));
            Assert.That(_context.Payments.Count(), Is.EqualTo(0));
        }

        [Test]
        public void When_BookFlight_SeatAlreadyBooked_ReturnsFailure()
        {
            _context.Bookings.Add(new Booking
            {
                Id = 2,
                UserId = 102,
                FlightId = 1,
                SeatNumber = "A1",
                BookingDate = DateTime.Now,
                Status = "PendingPayment"
            });

            _context.SaveChanges();

            BookFlightDto dto = new BookFlightDto
            {
                FlightId = 1,
                SeatNumber = "A1"
            };

            ApiResponse<object> result = _bookingService.BookFlight(dto, _user);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Seat already booked"));
            Assert.That(_context.Bookings.Count(), Is.EqualTo(1));
            Assert.That(_context.Payments.Count(), Is.EqualTo(0));


        }

        [Test]
        public void When_CancelBooking_ValidBooking_CancelsBookingSuccessfully()
        {
            _context.Bookings.Add(new Booking
            {
                Id = 5,
                UserId = 101,
                FlightId = 1,
                SeatNumber = "B1",
                BookingDate = DateTime.Now,
                Status = "PendingPayment"
            });

            _context.Payments.Add(new Payment
            {
                Id = 5,
                BookingId = 5,
                Amount = 5000,
                PaymentDate = DateTime.Now,
                Status = "Pending"
            });

            _context.Flights.First().AvailableSeats = 9;
            _context.SaveChanges();

            ApiResponse<string> result = _bookingService.CancelBooking(5);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Booking cancelled successfully"));
            Assert.That(_context.Bookings.First(b => b.Id == 5).Status, Is.EqualTo("Cancelled"));
            Assert.That(_context.Payments.First(p => p.BookingId == 5).Status, Is.EqualTo("Failed"));
            Assert.That(_context.Flights.First().AvailableSeats, Is.EqualTo(10));
        }

    }
}
