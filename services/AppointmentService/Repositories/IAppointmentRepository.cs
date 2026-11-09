using AppointmentService.Enums;
using AppointmentService.Models;

namespace AppointmentService.Repositories;

public interface IAppointmentRepository
{
    Task<int> CreateAppointmentAsync(Appointment appointment);

    Task<IEnumerable<Appointment>> GetAppointmentsAsync(
        int pageNumber,
        int pageSize);

    Task<Appointment?> GetAppointmentByIdAsync(int id);

    Task<bool> UpdateStatusAsync(
        int id,
        AppointmentStatus status);

    Task<bool> DeleteAppointmentAsync(int id);

    Task<bool> IsSlotBookedAsync(
    int doctorId,
    DateOnly date,
    TimeSpan time);
}
