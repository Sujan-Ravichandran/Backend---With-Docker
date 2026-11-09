using AppointmentService.Enums;

namespace AppointmentService.DTOs;

public class UpdateAppointmentRequest
{
    public AppointmentStatus Status { get; set; }
}