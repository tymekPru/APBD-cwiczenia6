using APBD_cwiczenia6.Models;

namespace APBD_cwiczenia6.Repositories
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetPatientsAsync(string? search);
        Task<Patient?> GetPatientByPeselAsync(string pesel);
    }
}