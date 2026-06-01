using Azure.Storage.Blobs;
using HospitalManagementApi.Models;
using HospitalManagementApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MedicalRecordsController : ControllerBase
	{
		private readonly IMedicalRecordRepository _repo;
		private readonly IConfiguration _config;

		public MedicalRecordsController(
			IMedicalRecordRepository repo,
			IConfiguration config)
		{
			_repo = repo;
			_config = config;
		}

		// GET: api/medicalrecords/patient/1
		[HttpGet("patient/{patientId}")]
		public async Task<IActionResult> GetByPatient(int patientId)
		{
			var records = await _repo.GetByPatientIdAsync(patientId);
			return Ok(records);
		}

		// POST: api/medicalrecords
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] MedicalRecord record)
		{
			await _repo.AddAsync(record);
			return Ok("Medical record save ho gaya!");
		}

		// POST: api/medicalrecords/upload
		[HttpPost("upload")]
		public async Task<IActionResult> UploadReport(
			IFormFile file, [FromQuery] int patientId)
		{
			var connectionString = _config
				.GetConnectionString("StorageConnection");
			var blobServiceClient = new BlobServiceClient(connectionString);
			var container = blobServiceClient
				.GetBlobContainerClient("medical-reports");

			var blobName = $"{patientId}-{file.FileName}";
			await container.UploadBlobAsync(
				blobName, file.OpenReadStream());

			return Ok($"Report uploaded! File: {blobName}");
		}
	}
}