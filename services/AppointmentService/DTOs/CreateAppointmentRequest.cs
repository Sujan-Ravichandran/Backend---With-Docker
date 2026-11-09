namespace AppointmentService.DTOs;

public class CreateAppointmentRequest
{
    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeSpan AppointmentTime { get; set; }
}                                                                                                                                                                           