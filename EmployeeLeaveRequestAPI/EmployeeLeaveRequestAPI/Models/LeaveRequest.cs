namespace EmployeeLeaveRequestAPI.Models
{
    public class LeaveRequest
    {
        public int LeaveRequestId { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeEmail { get; set; }

        public string MobileNumber { get; set; }

        public string LeaveType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; }

        public int TotalDays { get; set; }

        public string Status { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}