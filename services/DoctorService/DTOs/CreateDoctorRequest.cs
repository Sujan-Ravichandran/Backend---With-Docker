using System.ComponentModel.DataAnnotations;

namespace DoctorService.DTOs;

public class CreateDoctorRequest
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Specialization { get; set; } = string.Empty;

    [Required]
    public int Experience { get; set; }

    [Required]
    public TimeSpan AvailableFrom { get; set; }

    [Required]
    public TimeSpan AvailableTo { get; set; }
}