using HospitalManagementApi.Controllers;
using HospitalManagementApi.Models;
using Microsoft.Data.SqlClient;

namespace HospitalManagementApi.Repositories
{
	public class DoctorRepository : IDoctorRepository
	{
		private readonly string? _connectionString;

		public DoctorRepository(IConfiguration config)
		{
			_connectionString = config.GetConnectionString("HospitalDb");
		}

		public async Task<IEnumerable<Doctor>> GetAllAsync()
		{
			var doctors = new List<Doctor>();
			using var conn = new SqlConnection(_connectionString);
			await conn.OpenAsync();
			var cmd = new SqlCommand("SELECT * FROM Doctors", conn);
			var reader = await cmd.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				doctors.Add(new Doctor
				{
					DoctorId = (int)reader["DoctorId"],
					FullName = reader["FullName"].ToString(),
					Specialization = reader["Specialization"].ToString(),
					IsAvailable = (bool)reader["IsAvailable"]
				});
			}
			return doctors;
		}

		public async Task<IEnumerable<Doctor>> GetAvailableAsync()
		{
			var doctors = new List<Doctor>();
			using var conn = new SqlConnection(_connectionString);
			await conn.OpenAsync();
			var cmd = new SqlCommand(
				"SELECT * FROM Doctors WHERE IsAvailable = 1", conn);
			var reader = await cmd.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				doctors.Add(new Doctor
				{
					DoctorId = (int)reader["DoctorId"],
					FullName = reader["FullName"].ToString(),
					Specialization = reader["Specialization"].ToString(),
					IsAvailable = (bool)reader["IsAvailable"]
				});
			}
			return doctors;
		}

		public async Task AddAsync(Doctor doctor)
		{
			using var conn = new SqlConnection(_connectionString);
			await conn.OpenAsync();
			var cmd = new SqlCommand(
				@"INSERT INTO Doctors (FullName, Specialization, IsAvailable)
          VALUES (@FullName, @Specialization, @IsAvailable)", conn);
			cmd.Parameters.AddWithValue("@FullName", doctor.FullName ?? "");
			cmd.Parameters.AddWithValue("@Specialization", doctor.Specialization ?? "");
			cmd.Parameters.AddWithValue("@IsAvailable", doctor.IsAvailable);
			await cmd.ExecuteNonQueryAsync();
		}

		public async Task UpdateAsync(Doctor doctor)
		{
			using var conn = new SqlConnection(_connectionString);
			await conn.OpenAsync();
			var cmd = new SqlCommand(
				@"UPDATE Doctors SET 
            FullName = @FullName, 
            Specialization = @Specialization, 
            IsAvailable = @IsAvailable 
          WHERE DoctorId = @DoctorId", conn);
			cmd.Parameters.AddWithValue("@FullName", doctor.FullName ?? "");
			cmd.Parameters.AddWithValue("@Specialization", doctor.Specialization ?? "");
			cmd.Parameters.AddWithValue("@IsAvailable", doctor.IsAvailable);
			cmd.Parameters.AddWithValue("@DoctorId", doctor.DoctorId);
			await cmd.ExecuteNonQueryAsync();
		}
	}
}