namespace FleetMaintenanceApi.DTOs;

public class ApiResponseDto<T>
{
    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }
}
