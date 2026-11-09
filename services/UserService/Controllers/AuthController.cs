using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using UserService.DTOs;
using UserService.Models;
using UserService.Repositories;
using UserService.Services;
namespace UserService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly JwtService _jwtService;

    public AuthController(
        IUserRepository repository,
        JwtService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequest request)
    {
        var existingUser =
            await _repository.GetUserByEmailAsync(
                request.Email);

        if (existingUser is not null)
        {
            return BadRequest(
                "Email already exists");
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.Password),
            Role = request.Role,
            CreatedAt = DateTime.UtcNow
        };

        var userId =
            await _repository.CreateUserAsync(
                user);

        return Ok(new
        {
            Id = userId,
            Message = "User Created Successfully"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request)
    {
        var user =
            await _repository.GetUserByEmailAsync(
                request.Email);

        if (user == null)
        {
            return Unauthorized(
                "Invalid Credentials");
        }

        bool isValid =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

        if (!isValid)
        {
            return Unauthorized(
                "Invalid Credentials");
        }

        var token =
            _jwtService.GenerateToken(
                user.Id,
                user.Email,
                user.Role.ToString());

        return Ok(new
        {
            Token = token
        });
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(
        int id,
        RegisterRequest request)
    {
        Console.WriteLine($"Updating User Id: {id}");

        // var existingUser =
        //     await _repository.GetUserByEmailAsync(
        //         request.Email);

        var existingUser =
            await _repository.GetUserByIdAsync(id);

        if (existingUser == null)
        {
            return NotFound();
        }

        var user = new User
        {
            Id = id,
            Name = request.Name,
            Email = request.Email,
            Role = request.Role,
            PasswordHash = existingUser?.PasswordHash ?? "",
            UpdatedAt = DateTime.UtcNow
        };

        var updated =
            await _repository.UpdateUserAsync(user);

        if (!updated)
            return NotFound();

        return Ok("User Updated Successfully");
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(
        int id)
    {
        var deleted =
            await _repository.DeleteUserAsync(id);

        if (!deleted)
            return NotFound();

        return Ok("User Deleted Successfully");
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(
        int id)
    {
        var user =
            await _repository.GetUserByIdAsync(id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _repository.GetAllUsersAsync();

        return Ok(users);
    }
}