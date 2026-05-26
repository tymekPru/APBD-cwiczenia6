using APBD_cwiczenia6.DTOs;

namespace APBD_cwiczenia6.Services
{
    public interface IPatientService
    {
        Task<List<PatientDetailsResponseDto>> GetAllPatientsAsync(string? search);
    }
}