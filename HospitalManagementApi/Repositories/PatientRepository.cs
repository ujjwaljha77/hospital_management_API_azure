using Microsoft.Data.SqlClient;
using HospitalManagementApi.Models;

namespace HospitalManagementApi.Repositories
{
	public class PatientRepository : IPatientRepository
	{
		private readonly string? _connectionString;

		public PatientRepository(IConfiguration config)
		{
			_connectionString = config.GetConnectionString("HospitalDb");
		}

		public async Task<IEnumerable<Patient>> GetAllAsync()
		{
			var patients = new List<Patient>();
			using var conn = new SqlConnection(_connectionString);
			await conn.OpenAsync();
			var cmd = new SqlCommand("SELECT * FROM Patients", conn);
			var reader = await cmd.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				patients.Add(new Patient
				{
					PatientId = (int)reader["PatientId"],
					FullName = reader["FullName"].ToString(),
					Email = reader["Email"].ToString(),
					Phone = reader["Phone"].ToString(),
					DateOfBirth = (DateTime)reader["DateOfBirth"]
				});
			}
			return patients;
		}

		public async Task<Patient?> GetByIdAsync(int id)
		{
			using var conn = new SqlConnection(_connectionString);
			await conn.OpenAsync();
			var cmd = new SqlCommand(
				"SELECT * FROM Patients WHERE PatientId = @Id", conn);
			cmd.Parameters.AddWithValue("@Id", id);
			var reader = await cmd.ExecuteReaderAsync();
			if (await reader.ReadAsync())
			{
				return new Patient
				{
					PatientId = (int)reader["PatientId"],
					FullName = reader["FullName"].ToString(),
					Email = reader["Email"].ToString(),
					Phone = reader["Phone"].ToString(),
					DateOfBirth = (DateTime)reader["DateOfBirth"]
				};
			}
			return null;
		}

		public async Task AddAsync(Patient patient)
		{
			using var conn = new SqlConnection(_connectionString);
			await conn.OpenAsync();
			var cmd = new SqlCommand(
				@"INSERT INTO Patients (FullName, Email, Phone, DateOfBirth) 
                  VALUES (@FullName, @Email, @Phone, @DateOfBirth)", conn);
			cmd.Parameters.AddWithValue("@FullName", patient.FullName);
			cmd.Parameters.AddWithValue("@Email", patient.Email);
			cmd.Parameters.AddWithValue("@Phone", patient.Phone);
			cmd.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
			await cmd.ExecuteNonQueryAsync();
		}

		public async Task DeleteAsync(int id)
		{
			using var conn = new SqlConnection(_connectionString);
			await conn.OpenAsync();
			var cmd = new SqlCommand(
				"DELETE FROM Patients WHERE PatientId = @Id", conn);
			cmd.Parameters.AddWithValue("@Id", id);
			await cmd.ExecuteNonQueryAsync();
		}

	}
}