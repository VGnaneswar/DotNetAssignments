using EmployeeLeaveRequestAPI.DTOs;

namespace EmployeeLeaveRequestAPI.Services
{
    public interface ILeaveRequestService
    {
        LeaveRequestResponseDto CreateLeaveRequest(
            LeaveRequestCreateDto dto);

        List<LeaveRequestResponseDto> GetAllLeaveRequests();

        LeaveRequestResponseDto GetLeaveRequestById(int id);
    }
}