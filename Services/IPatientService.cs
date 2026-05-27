using APBD_cwiczenia6.DTOs;
using APBD_cwiczenia6.Models;

namespace APBD_cwiczenia6.Services
{
    public interface IPatientService
    {
        Task<List<PatientDetailsResponseDto>> GetAllPatientsAsync(string? search);
        Task<BedAssignmentResponseDto> TryAssignBedToPatientAsync(string pesel, BedAssignmentRequestDto data);
    }
}