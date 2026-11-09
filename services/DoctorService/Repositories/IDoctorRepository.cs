using DoctorService.Models;

namespace DoctorService.Repositories;

public interface IDoctorRepository
{
    Task<int> CreateDoctorAsync(Doctor doctor);

    Task<IEnumerable<Doctor>> GetAllDoctorsAsync();

    Task<Doctor?> GetDoctorByIdAsync(int id);

    Task<bool> UpdateDoctorAsync(
    Doctor doctor);

    Task<Doctor?> GetDoctorByUserIdAsync(int userId);

    Task<IEnumerable<Doctor>> GetDoctorsAsync(
    int pageNumber,
    int pageSize);

    Task<bool> DeleteDoctorAsync(int id);
}