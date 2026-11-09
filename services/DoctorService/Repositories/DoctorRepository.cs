using Dapper;
using DoctorService.Data;
using DoctorService.Models;

namespace DoctorService.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly DapperContext _context;

    public DoctorRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<int> CreateDoctorAsync(Doctor doctor)
    {
        var query = @"
        INSERT INTO Doctors
        (
            UserId,
            Name,
            Specialization,
            Experience,
            AvailableFrom,
            AvailableTo,
            CreatedAt
        )
        VALUES
        (
            @UserId,
            @Name,
            @Specialization,
            @Experience,
            @AvailableFrom,
            @AvailableTo,
            @CreatedAt
        )
        RETURNING Id";

        using var connection =
            _context.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            query,
            doctor);
    }

    public async Task<IEnumerable<Doctor>>
        GetAllDoctorsAsync()
    {
        using var connection =
            _context.CreateConnection();

        return await connection.QueryAsync<Doctor>(
            "SELECT * FROM Doctors");
    }

    public async Task<Doctor?>
        GetDoctorByIdAsync(int id)
    {
        using var connection =
            _context.CreateConnection();

        return await connection
            .QueryFirstOrDefaultAsync<Doctor>(
            "SELECT * FROM Doctors WHERE Id=@Id",
            new { Id = id });
    }
    public async Task<IEnumerable<Doctor>>
    GetDoctorsAsync(
        int pageNumber,
        int pageSize)
    {
        var query = @"
            SELECT *
            FROM Doctors
            ORDER BY Id
            LIMIT @PageSize
            OFFSET @Offset";

        var offset =
            (pageNumber - 1) * pageSize;

        using var connection =
            _context.CreateConnection();

        return await connection.QueryAsync<Doctor>(
            query,
            new
            {
                PageSize = pageSize,
                Offset = offset
            });
    }
    
    public async Task<bool> DeleteDoctorAsync(int id)
    {
        var query =
            "DELETE FROM Doctors WHERE Id = @Id";

        using var connection =
            _context.CreateConnection();

        var rows =
            await connection.ExecuteAsync(
                query,
                new { Id = id });

        return rows > 0;
    }

    public async Task<Doctor?> GetDoctorByUserIdAsync(
    int userId)
    {
        var query =
            "SELECT * FROM Doctors WHERE UserId=@UserId";

        using var connection =
            _context.CreateConnection();

        return await connection
            .QueryFirstOrDefaultAsync<Doctor>(
                query,
                new { UserId = userId });
    }
    public async Task<bool> UpdateDoctorAsync(
    Doctor doctor)
{
    var query = @"
        UPDATE Doctors
        SET
            Name=@Name,
            Specialization=@Specialization,
            Experience=@Experience,
            AvailableFrom=@AvailableFrom,
            AvailableTo=@AvailableTo
        WHERE Id=@Id";

    using var connection =
            _context.CreateConnection();

        var rows =
            await connection.ExecuteAsync(
                query,
                doctor);

        return rows > 0;
    }
}
