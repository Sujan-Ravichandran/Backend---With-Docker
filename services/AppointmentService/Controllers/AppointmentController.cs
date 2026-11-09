using AppointmentService.DTOs;
using AppointmentService.Enums;
using AppointmentService.Models;
using AppointmentService.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
namespace AppointmentService.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentController
    : ControllerBase
{
    private readonly IHttpClientFactory
    _httpClientFactory;

    private readonly IAppointmentRepository
        _repository;

    public AppointmentController(
        IAppointmentRepository repository,
        IHttpClientFactory httpClientFactory)
    {
        _repository = repository;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost]
public async Task<IActionResult> Create(
    CreateAppointmentRequest request)
{
    var client =
        _httpClientFactory.CreateClient();

    // USER SERVICE

    var userResponse =
        await client.GetFromJsonAsync<UserResponse>(
        $"http://userservice:8080/api/auth/{request.PatientId}");

    if (userResponse == null)
    {
        return BadRequest(
            "Patient not found");
    }

    if (userResponse.Role != "Patient")
    {
        return BadRequest(
            "Only patients can book appointments");
    }

    // DOCTOR SERVICE

    var doctorResponse =
        await client.GetFromJsonAsync<DoctorResponse>(
        $"http://doctorservice:8080/api/doctors/{request.DoctorId}");

    if (doctorResponse == null)
    {
        return BadRequest(
            "Doctor not found");
    }

    // AVAILABILITY

    if (request.AppointmentTime <
        doctorResponse.AvailableFrom ||
        request.AppointmentTime >
        doctorResponse.AvailableTo)
    {
        return BadRequest(
            "Doctor is not available at this time");
    }

    // SLOT VALIDATION

    var isBooked =
        await _repository.IsSlotBookedAsync(
            request.DoctorId,
            request.AppointmentDate,
            request.AppointmentTime);

    if (isBooked)
    {
        return BadRequest(
            "Appointment slot already booked");
    }

    // CREATE APPOINTMENT

    var appointment =
        new Appointment
        {
            PatientId =
                request.PatientId,

            DoctorId =
                request.DoctorId,

            AppointmentDate =
                request.AppointmentDate,

            AppointmentTime =
                request.AppointmentTime,

            Status =
                AppointmentStatus.Pending,

            CreatedAt =
                DateTime.UtcNow
        };

    var id =
        await _repository
            .CreateAppointmentAsync(
                appointment);

    return Ok(new
    {
        Id = id,
        Message =
            "Appointment Created Successfully"
    });
}

    [HttpGet]
    public async Task<IActionResult> GetAll(
        int pageNumber = 1,
        int pageSize = 10)
    {
        var appointments =
            await _repository.GetAppointmentsAsync(
                pageNumber,
                pageSize);

        return Ok(appointments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        int id)
    {
        var appointment =
            await _repository
                .GetAppointmentByIdAsync(id);

        if (appointment == null)
            return NotFound();

        return Ok(appointment);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateAppointmentRequest request)
    {
        var updated =
            await _repository
                .UpdateStatusAsync(
                    id,
                    request.Status);

        if (!updated)
            return NotFound();

        return Ok(
            "Appointment Updated");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id)
    {
        var deleted =
            await _repository
                .DeleteAppointmentAsync(id);

        if (!deleted)
            return NotFound();

        return Ok(
            "Appointment Deleted");
    }
}