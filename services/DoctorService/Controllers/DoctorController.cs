using Microsoft.AspNetCore.Mvc;
using DoctorService.DTOs;
using DoctorService.Models;
using DoctorService.Repositories;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
namespace DoctorService.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorController : ControllerBase
{
    private readonly IDoctorRepository _repository;

    private readonly IHttpClientFactory _httpClientFactory;

    public DoctorController(
        IDoctorRepository repository,
        IHttpClientFactory httpClientFactory)
    {
        _repository = repository;
        _httpClientFactory = httpClientFactory;
    }

    // public DoctorController(
    //     IDoctorRepository repository)
    // {
    //     _repository = repository;
    // }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateDoctorRequest request)
    {
        var client =
            _httpClientFactory.CreateClient();

        var user =
            await client.GetFromJsonAsync<UserResponse>(
                $"http://userservice:8080/api/auth/{request.UserId}");

        if (user == null)
        {
            return BadRequest("User not found");
        }

        if (user.Role != "Doctor")
        {
            return BadRequest(
                "Only users with Doctor role can create doctor profile");
        }

        var existingDoctor =
            await _repository.GetDoctorByUserIdAsync(
                request.UserId);

        if (existingDoctor != null)
        {
            return BadRequest(
                "Doctor profile already exists");
        }

        var doctor = new Doctor
        {
            UserId = request.UserId,
            Name = request.Name,
            Specialization = request.Specialization,
            Experience = request.Experience,
            AvailableFrom = request.AvailableFrom,
            AvailableTo = request.AvailableTo,
            CreatedAt = DateTime.UtcNow
        };

        var id =
            await _repository.CreateDoctorAsync(
                doctor);

        return Ok(new
        {
            Id = id,
            Message = "Doctor Created Successfully"
        });
    }
    

    // [HttpGet]
    // public async Task<IActionResult>
    //     GetAll()
    // {
    //     return Ok(
    //         await _repository
    //             .GetAllDoctorsAsync());
    // }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        int pageNumber = 1,
        int pageSize = 10)
    {
        var doctors =
            await _repository.GetDoctorsAsync(
                pageNumber,
                pageSize);

        return Ok(doctors);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult>
        GetById(int id)
    {
        var doctor =
            await _repository
                .GetDoctorByIdAsync(id);

        if (doctor == null)
            return NotFound();

        

        return Ok(doctor);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor(
        int id)
    {
        var deleted =
            await _repository.DeleteDoctorAsync(id);

        if (!deleted)
        {
            return NotFound(
                $"Doctor with Id {id} not found");
        }

        return Ok(new
        {
            Message = "Doctor Deleted Successfully"
        });
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDoctor(
        int id,
        CreateDoctorRequest request)
    {
        var doctor = await _repository.GetDoctorByIdAsync(id);

        if (doctor == null)
        {
            return NotFound();
        }

        doctor.Name = request.Name;
        doctor.Specialization = request.Specialization;
        doctor.Experience = request.Experience;
        doctor.AvailableFrom = request.AvailableFrom;
        doctor.AvailableTo = request.AvailableTo;

        var updated =
            await _repository.UpdateDoctorAsync(doctor);

        if (!updated)
        {
            return NotFound();
        }

        return Ok("Doctor Updated Successfully");
    }
}

