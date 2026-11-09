using AppointmentService.Data;
using AppointmentService.Enums;
using AppointmentService.Models;
using Dapper;

namespace AppointmentService.Repositories;

public class AppointmentRepository
    : IAppointmentRepository
{
    private readonly DapperContext _context;

    public AppointmentRepository(
        DapperContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAppointmentAsync(
        Appointment appointment)
    {
        var query = @"
            INSERT INTO Appointments
            (
                PatientId,
                DoctorId,
                AppointmentDate,
                AppointmentTime,
                Status,
                CreatedAt
            )
            VALUES
            (
                @PatientId,
                @DoctorId,
                @AppointmentDate,
                @AppointmentTime,
                @Status,
                @CreatedAt
            )
            RETURNING Id";

        using var connection =
            _context.CreateConnection();

        return await connection
            .ExecuteScalarAsync<int>(
                query,
                appointment);
    }

    public async Task<IEnumerable<Appointment>>
        GetAppointmentsAsync(
            int pageNumber,
            int pageSize)
    {
        var query = @"
            SELECT *
            FROM Appointments
            ORDER BY Id
            LIMIT @PageSize
            OFFSET @Offset";

        var offset =
            (pageNumber - 1) * pageSize;

        using var connection =
            _context.CreateConnection();

        return await connection
            .QueryAsync<Appointment>(
                query,
                new
                {
                    PageSize = pageSize,
                    Offset = offset
                });
    }

    public async Task<Appointment?>
        GetAppointmentByIdAsync(int id)
    {
        var query =
            @"SELECT *
              FROM Appointments
              WHERE Id = @Id";

        using var connection =
            _context.CreateConnection();

        return await connection
            .QueryFirstOrDefaultAsync<Appointment>(
                query,
                new { Id = id });
    }

    public async Task<bool> UpdateStatusAsync(
        int id,
        AppointmentStatus status)
    {
        var query = @"
            UPDATE Appointments
            SET
                Status = @Status,
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id";

        using var connection =
            _context.CreateConnection();

        var rows =
            await connection.ExecuteAsync(
                query,
                new
                {
                    Id = id,
                    Status = status.ToString(),
                    UpdatedAt = DateTime.UtcNow
                });

        return rows > 0;
    }

    public async Task<bool> DeleteAppointmentAsync(
        int id)
    {
        var query =
            @"DELETE
              FROM Appointments
              WHERE Id = @Id";

        using var connection =
            _context.CreateConnection();

        var rows =
            await connection.ExecuteAsync(
                query,
                new { Id = id });

        return rows > 0;
    }
    public async Task<bool> IsSlotBookedAsync(
        int doctorId,
        DateOnly date,
        TimeSpan time)
    {
        var query = @"
        SELECT COUNT(*)
        FROM Appointments
        WHERE DoctorId = @DoctorId
        AND AppointmentDate = @AppointmentDate
        AND AppointmentTime = @AppointmentTime";

        using var connection =
            _context.CreateConnection();

        var count =
            await connection.ExecuteScalarAsync<int>(
                query,
                new
                {
                    DoctorId = doctorId,
                    AppointmentDate = date,
                    AppointmentTime = time
                });

        return count > 0;
    }
}
