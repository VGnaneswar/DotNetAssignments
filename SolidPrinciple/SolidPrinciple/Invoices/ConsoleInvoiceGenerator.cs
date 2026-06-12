using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartCourierApp.Models;

namespace SmartCourierApp.Invoices
{
    public class ConsoleInvoiceGenerator : IInvoiceGenerator
    {
        public void GenerateInvoice(CourierBooking booking)
        {
            
            Console.WriteLine($"Customer Name      : {booking.Customer.Name}");
            Console.WriteLine($"Source City        : {booking.Parcel.SourceCity}");
            Console.WriteLine($"Destination City   : {booking.Parcel.DestinationCity}");
            Console.WriteLine($"Parcel Weight      : {booking.Parcel.Weight} Kg");
            Console.WriteLine($"Delivery Type      : {booking.DeliveryType}");
            Console.WriteLine($"Delivery Charge    : Rs.{booking.DeliveryCharge}");
            
        }
    }
}