namespace DoctorService.Models;

public class Doctor
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public required string Name { get; set; }

    public required string Specialization { get; set; }

    public int Experience { get; set; }

    public TimeSpan AvailableFrom { get; set; }

    public TimeSpan AvailableTo { get; set; }

    public DateTime CreatedAt { get; set; }
}