using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartCourierApp.Notifications
{
    public class WhatsAppNotificationService : INotificationService
    {
        public void SendNotification(string message)
        {
            Console.WriteLine($"WhatsApp Notification: {message}");
        }
    }
}