using Microsoft.AspNetCore.Mvc;
using EmployeeLeaveRequestAPI.DTOs;
using EmployeeLeaveRequestAPI.Services;
namespace EmployeeLeaveRequestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ILeaveRequestService _service;

        public LeaveRequestsController(
            ILeaveRequestService service)
        {
            _service = service;
        }
        [HttpPost]
        public ActionResult<LeaveRequestResponseDto> CreateLeaveRequest(LeaveRequestCreateDto dto)
        {
            var result = _service.CreateLeaveRequest(dto);

            return CreatedAtAction(
                nameof(GetLeaveRequestById),
                new { id = result.LeaveRequestId },
                result);
        }
        [HttpGet]
        public ActionResult<List<LeaveRequestResponseDto>> GetAllLeaveRequests()
        {
            var leaveRequests = _service.GetAllLeaveRequests();

            return Ok(leaveRequests);
        }
        [HttpGet("{id}")]
        public ActionResult<LeaveRequestResponseDto> GetLeaveRequestById(int id)
        {
            var leaveRequest = _service.GetLeaveRequestById(id);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            return Ok(leaveRequest);
        }
    }

}