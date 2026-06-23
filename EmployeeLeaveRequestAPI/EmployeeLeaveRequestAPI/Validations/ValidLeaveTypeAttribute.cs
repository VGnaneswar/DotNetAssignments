using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveRequestAPI.Validations
{
    public class ValidLeaveTypeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Leave Type is required.");
            }

            string leaveType = value.ToString();

            string[] validTypes =
            {
                "Sick",
                "Casual",
                "Earned"
            };

            if (validTypes.Contains(leaveType))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(
                "Leave Type must be Sick, Casual, or Earned.");
        }
    }
}