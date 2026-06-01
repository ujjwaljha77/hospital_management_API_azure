using Azure.Messaging.ServiceBus;
using HospitalManagementApi.Models;
using HospitalManagementApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HospitalManagementApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AppointmentsController : ControllerBase
	{
		private readonly IAppointmentRepository _repo;
		private readonly IConfiguration _config;

		public AppointmentsController(IAppointmentRepository repo, IConfiguration config)
		{
			_repo = repo;
			_config = config;
		}

		// GET: api/appointments
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var appointments = await _repo.GetAllAsync();
			return Ok(appointments);
		}

		// GET: api/appointments/1
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var appointment = await _repo.GetByIdAsync(id);
			if (appointment == null)
				return NotFound("Appointment nahi mila!");
			return Ok(appointment);
		}

		// POST: api/appointments
		[HttpPost]
		public async Task<IActionResult> Book([FromBody] Appointment appointment)
		{
			// 1. DB mein save karo
			await _repo.AddAsync(appointment);

			// 2. Service Bus Queue mein message bhejo
			var connectionString = _config.GetConnectionString("ServiceBus");
			var queueName = _config["ServiceBus:QueueName"];

			await using var client = new ServiceBusClient(connectionString);
			var sender = client.CreateSender(queueName);

			var messageBody = JsonSerializer.Serialize(new
			{
				PatientId = appointment.PatientId,
				DoctorId = appointment.DoctorId,
				AppointmentDate = appointment.AppointmentDate,
				Status = "Confirmed",
				PatientEmail = appointment.PatientEmail
			});

			await sender.SendMessageAsync(new ServiceBusMessage(messageBody));

			return Ok("Appointment book ho gaya! Confirmation email aa jayega.");
		}
	}
}