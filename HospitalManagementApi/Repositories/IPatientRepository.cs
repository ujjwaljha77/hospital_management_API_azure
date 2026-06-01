using HospitalManagementApi.Models;

namespace HospitalManagementApi.Repositories
{
	public interface IPatientRepository
	{
		Task<IEnumerable<Patient>> GetAllAsync();
		Task<Patient?> GetByIdAsync(int id);
		Task AddAsync(Patient patient);
		Task DeleteAsync(int id);
	}
}