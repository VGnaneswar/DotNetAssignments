using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMembershipApp.Interfaces
{
    public interface INotificationService
    {
        void SendBorrowNotification(string email, string bookTitle);
    }
}