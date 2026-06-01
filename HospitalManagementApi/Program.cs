using Azure.Identity;
using HospitalManagementApi.Repositories;

namespace HospitalManagementApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var keyVaultUrl = "https://ujjwal001.vault.azure.net/";

			// System Assigned (Implemented).
			var credential = new ManagedIdentityCredential();
			builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), credential);


			// Add services to the container.
			builder.Services.AddControllers();
			builder.Services.AddScoped<IPatientRepository, PatientRepository>();
			builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
			builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
			builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			app.MapGet("/", () => "Welcome to Hospital Management API!");
			app.UseSwagger();
			app.UseSwaggerUI();
			app.UseAuthorization();
			app.MapControllers();
			app.Run();
		}
	}
}