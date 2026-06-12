using SmartCourierApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SmartCourierApp.Models;

namespace SmartCourierApp.Invoices
{
    public interface IInvoiceGenerator
    {
        void GenerateInvoice(CourierBooking booking);
    }
}