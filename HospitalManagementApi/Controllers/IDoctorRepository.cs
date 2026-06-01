using HospitalManagementApi.Models;

namespace HospitalManagementApi.Repositories
{
	public interface IDoctorRepository
	{
		Task<IEnumerable<Doctor>> GetAllAsync();
		Task<IEnumerable<Doctor>> GetAvailableAsync();
		Task AddAsync(Doctor doctor);
		Task UpdateAsync(Doctor doctor);
	}
}