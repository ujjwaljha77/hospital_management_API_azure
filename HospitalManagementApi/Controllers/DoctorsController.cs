using HospitalManagementApi.Models;
using HospitalManagementApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DoctorsController : ControllerBase
	{
		private readonly IDoctorRepository _repo;

		public DoctorsController(IDoctorRepository repo)
		{
			_repo = repo;
		}

		// GET: api/doctors
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var doctors = await _repo.GetAllAsync();
			return Ok(doctors);
		}

		// GET: api/doctors/available
		[HttpGet("available")]
		public async Task<IActionResult> GetAvailable()
		{
			var doctors = await _repo.GetAvailableAsync();
			return Ok(doctors);
		}

		// POST: api/doctors
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] Doctor doctor)
		{
			await _repo.AddAsync(doctor);
			return Ok("Doctor successfully add ho gaya!");
		}

		// PUT: api/doctors
		[HttpPut]
		public async Task<IActionResult> Update([FromBody] Doctor doctor)
		{
			await _repo.UpdateAsync(doctor);
			return Ok("Doctor update ho gaya!");
		}
	}
}