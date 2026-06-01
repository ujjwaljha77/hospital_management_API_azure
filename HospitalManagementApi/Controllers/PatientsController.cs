using HospitalManagementApi.Models;
using HospitalManagementApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PatientsController : ControllerBase
	{
		private readonly IPatientRepository _repo;

		public PatientsController(IPatientRepository repo)
		{
			_repo = repo;
		}

		// GET: api/patients
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var patients = await _repo.GetAllAsync();
			return Ok(patients);
		}

		// GET: api/patients/1
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var patient = await _repo.GetByIdAsync(id);
			if (patient == null)
				return NotFound("Patient nahi mila!");
			return Ok(patient);
		}

		// POST: api/patients
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] Patient patient)
		{
			await _repo.AddAsync(patient);
			return Ok("Patient successfully add ho gaya!");
		}

		// DELETE: api/patients/1
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _repo.DeleteAsync(id);
			return Ok("Patient delete ho gaya!");
		}
	}
}