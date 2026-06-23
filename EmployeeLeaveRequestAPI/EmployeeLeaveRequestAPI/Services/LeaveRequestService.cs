using EmployeeLeaveRequestAPI.DTOs;
using EmployeeLeaveRequestAPI.Models;

namespace EmployeeLeaveRequestAPI.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private static List<LeaveRequest> leaveRequests = new List<LeaveRequest>();

        public LeaveRequestResponseDto CreateLeaveRequest(LeaveRequestCreateDto dto)
        {
            LeaveRequest leaveRequest = new LeaveRequest
            {
                LeaveRequestId = leaveRequests.Count + 1,

                EmployeeName = dto.EmployeeName,

                EmployeeEmail = dto.EmployeeEmail,

                MobileNumber = dto.MobileNumber,

                LeaveType = dto.LeaveType,

                StartDate = dto.StartDate,

                EndDate = dto.EndDate,

                Reason = dto.Reason,

                TotalDays = (dto.EndDate - dto.StartDate).Days + 1,

                Status = "Pending",

                CreatedOn = DateTime.Now
            };

            leaveRequests.Add(leaveRequest);

            return new LeaveRequestResponseDto
            {
                LeaveRequestId = leaveRequest.LeaveRequestId,

                EmployeeName = leaveRequest.EmployeeName,

                EmployeeEmail = leaveRequest.EmployeeEmail,

                LeaveType = leaveRequest.LeaveType,

                StartDate = leaveRequest.StartDate,

                EndDate = leaveRequest.EndDate,

                Reason = leaveRequest.Reason,

                TotalDays = leaveRequest.TotalDays,

                Status = leaveRequest.Status,

                CreatedOn = leaveRequest.CreatedOn
            };
        }

        public List<LeaveRequestResponseDto> GetAllLeaveRequests()
        {
            return leaveRequests.Select(lr => new LeaveRequestResponseDto
            {
                LeaveRequestId = lr.LeaveRequestId,
                EmployeeName = lr.EmployeeName,
                EmployeeEmail = lr.EmployeeEmail,
                LeaveType = lr.LeaveType,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                Reason = lr.Reason,
                TotalDays = lr.TotalDays,
                Status = lr.Status,
                CreatedOn = lr.CreatedOn
            }).ToList();
        
        }
        public LeaveRequestResponseDto GetLeaveRequestById(int id)
        {
            LeaveRequest leaveRequest =
                leaveRequests.FirstOrDefault(
                    lr => lr.LeaveRequestId == id);

            if (leaveRequest == null)
            {
                return null;
            }

            return new LeaveRequestResponseDto
            {
                LeaveRequestId = leaveRequest.LeaveRequestId,
                EmployeeName = leaveRequest.EmployeeName,
                EmployeeEmail = leaveRequest.EmployeeEmail,
                LeaveType = leaveRequest.LeaveType,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Reason = leaveRequest.Reason,
                TotalDays = leaveRequest.TotalDays,
                Status = leaveRequest.Status,
                CreatedOn = leaveRequest.CreatedOn
            };
        }
    }
}