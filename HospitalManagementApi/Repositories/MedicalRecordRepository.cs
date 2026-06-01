using HospitalManagementApi.Models;
using Microsoft.Data.SqlClient;

namespace HospitalManagementApi.Repositories
{
	public class MedicalRecordRepository : IMedicalRecordRepository
	{
		private readonly string? _connectionString;

		public MedicalRecordRepository(IConfiguration config)
		{
			_connectionString = config.GetConnectionString("HospitalDb");
		}

		public async Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId)
		{
			var list = new List<MedicalRecord>();
			using var conn = new SqlConnection(_connectionString);
			await conn.OpenAsync();
			var cmd = new SqlCommand(
				"SELECT * FROM MedicalRecords WHERE PatientId = @PatientId", conn);
			cmd.Parameters.AddWithValue("@PatientId", patientId);
			var reader = await cmd.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				list.Add(new MedicalRecord
				{
					RecordId = (int)reader["RecordId"],
					PatientId = (int)reader["PatientId"],
					DoctorId = (int)reader["DoctorId"],
					Diagnosis = reader["Diagnosis"].ToString(),
					Prescription = reader["Prescription"].ToString(),
					RecordDate = (DateTime)reader["RecordDate"]
				});
			}
			return list;
		}

		public async Task AddAsync(MedicalRecord record)
		{
			using var conn = new SqlConnection(_connectionString);
			await conn.OpenAsync();
			var cmd = new SqlCommand(
				@"INSERT INTO MedicalRecords (PatientId, DoctorId, Diagnosis, Prescription)
                  VALUES (@PatientId, @DoctorId, @Diagnosis, @Prescription)", conn);
			cmd.Parameters.AddWithValue("@PatientId", record.PatientId);
			cmd.Parameters.AddWithValue("@DoctorId", record.DoctorId);
			cmd.Parameters.AddWithValue("@Diagnosis", record.Diagnosis ?? "");
			cmd.Parameters.AddWithValue("@Prescription", record.Prescription ?? "");
			await cmd.ExecuteNonQueryAsync();
		}
	}
}