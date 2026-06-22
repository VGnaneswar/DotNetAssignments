using SimplyFly.API.Data;
using SimplyFly.API.DTOs;
using SimplyFly.API.Models;
using SimplyFly.API.Services.Interfaces;

namespace SimplyFly.API.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApiResponse<object> MakePayment(MakePaymentDto dto)
        {
            var booking = _context.Bookings
                .FirstOrDefault(b => b.Id == dto.BookingId);

            if (booking == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Booking not found"
                };
            }

            if (booking.Status == "Cancelled")
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Cancelled booking cannot be paid"
                };
            }

            if (booking.Status == "Confirmed")
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Booking is already confirmed"
                };
            }

            var payment = _context.Payments
                .FirstOrDefault(p => p.BookingId == dto.BookingId);

            if (payment == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Payment record not found"
                };
            }

            if (payment.Status == "Paid")
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Payment already completed"
                };
            }

            payment.Status = "Paid";
            payment.PaymentDate = DateTime.Now;

            booking.Status = "Confirmed";

            _context.SaveChanges();

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Payment successful and booking confirmed",
                Data = new
                {
                    BookingId = booking.Id,
                    BookingStatus = booking.Status,
                    PaymentId = payment.Id,
                    PaymentStatus = payment.Status,
                    Amount = payment.Amount
                }
            };
        }
    }
}
