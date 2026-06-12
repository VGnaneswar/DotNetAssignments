using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMembershipApp.Models
{
    public class Member
    {
        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public int BorrowedBookCount { get; set; }

        public bool IsPremiumMember { get; set; }
    }
}