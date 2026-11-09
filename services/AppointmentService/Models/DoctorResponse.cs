namespace AppointmentService.Models;

public class DoctorResponse
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public int Experience { get; set; }

    public TimeSpan AvailableFrom { get; set; }

    public TimeSpan AvailableTo { get; set; }
}