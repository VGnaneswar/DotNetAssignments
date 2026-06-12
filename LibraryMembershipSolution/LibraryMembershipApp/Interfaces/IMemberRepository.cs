using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryMembershipApp.Models;

namespace LibraryMembershipApp.Interfaces
{
    public interface IMemberRepository
    {
        Member? GetMemberById(int memberId);

        void UpdateBorrowedBookCount(int memberId);
    }
}