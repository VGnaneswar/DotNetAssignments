using SmartCourierApp.Models;
using SmartCourierApp.DeliveryCalculators;
using SmartCourierApp.Notifications;
using SmartCourierApp.Invoices;
using SmartCourierApp.Services;




Console.Write("Enter Customer Name: ");
string name = Console.ReadLine();

Console.Write("Enter Customer Email: ");
string email = Console.ReadLine();

Console.Write("Enter Mobile Number: ");
string mobile = Console.ReadLine();


Console.Write("Enter Parcel Weight (Kg): ");
double weight = Convert.ToDouble(Console.ReadLine());

Console.Write("Enter Source City: ");
string source = Console.ReadLine();

Console.Write("Enter Destination City: ");
string destination = Console.ReadLine();


Console.WriteLine("\nSelect Delivery Type:");
Console.WriteLine("1. Standard");
Console.WriteLine("2. Express");
Console.WriteLine("3. International");

int deliveryChoice = Convert.ToInt32(Console.ReadLine());

IDeliveryChargeCalculator calculator = deliveryChoice switch
{
    1 => new StandardDeliveryCalculator(),
    2 => new ExpressDeliveryCalculator(),
    3 => new InternationalDeliveryCalculator(),
    _ => new StandardDeliveryCalculator()
};


Console.WriteLine("\nSelect Notification Type:");
Console.WriteLine("1. Email");
Console.WriteLine("2. SMS");
Console.WriteLine("3. WhatsApp");

int notificationChoice = Convert.ToInt32(Console.ReadLine());

INotificationService notificationService = notificationChoice switch
{
    1 => new EmailNotificationService(),
    2 => new SmsNotificationService(),
    3 => new WhatsAppNotificationService(),
    _ => new EmailNotificationService()
};

string deliveryType = deliveryChoice switch
{
    1 => "Standard",
    2 => "Express",
    3 => "International",
    _ => "Standard"
};

string notificationType = notificationChoice switch
{
    1 => "Email",
    2 => "SMS",
    3 => "WhatsApp",
    _ => "Email"
};


Customer customer = new Customer
{
    Name = name,
    Email = email,
    MobileNumber = mobile
};

Parcel parcel = new Parcel
{
    Weight = weight,
    SourceCity = source,
    DestinationCity = destination
};

CourierBooking booking = new CourierBooking
{
    Customer = customer,
    Parcel = parcel,
    DeliveryType = deliveryType,
    NotificationType = notificationType
};

IInvoiceGenerator invoiceGenerator =
    new ConsoleInvoiceGenerator();

CourierBookingService bookingService =
    new CourierBookingService(
        calculator,
        notificationService,
        invoiceGenerator);

bookingService.BookCourier(booking);

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();