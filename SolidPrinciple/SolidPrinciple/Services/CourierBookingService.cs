using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartCourierApp.Models;
using SmartCourierApp.DeliveryCalculators;
using SmartCourierApp.Notifications;
using SmartCourierApp.Invoices;

namespace SmartCourierApp.Services
{
    public class CourierBookingService
    {
        private readonly IDeliveryChargeCalculator _calculator;
        private readonly INotificationService _notificationService;
        private readonly IInvoiceGenerator _invoiceGenerator;

        public CourierBookingService(
            IDeliveryChargeCalculator calculator,
            INotificationService notificationService,
            IInvoiceGenerator invoiceGenerator)
        {
            _calculator = calculator;
            _notificationService = notificationService;
            _invoiceGenerator = invoiceGenerator;
        }

        public void BookCourier(CourierBooking booking)
        {
            booking.DeliveryCharge =
                _calculator.CalculateCharge(booking.Parcel.Weight);

            _notificationService.SendNotification(
                "Courier booking confirmed successfully."
            );

            _invoiceGenerator.GenerateInvoice(booking);
        }
    }
}