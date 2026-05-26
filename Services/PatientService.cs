using APBD_cwiczenia6.DTOs;
using APBD_cwiczenia6.Models;
using APBD_cwiczenia6.Repositories;

namespace APBD_cwiczenia6.Services
{
    public class PatientService(IPatientRepository patientRepository) : IPatientService
    {
        private readonly IPatientRepository _patientRepository = patientRepository;

        public async Task<List<PatientDetailsResponseDto>> GetAllPatientsAsync(string? search)
        {
            var patients = await _patientRepository.GetPatientsAsync(search);

            return [.. patients.Select(p => new PatientDetailsResponseDto
            {
                Pesel = p.Pesel,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Age = p.Age,
                Sex = p.Sex ? "Male" : "Female",

                Admissions = [.. p.Admissions.Select(a => new AdmissionResponseDto
                {
                    Id = a.Id,
                    AdmissionDate = a.AdmissionDate,
                    DischargeDate = a.DischargeDate,
                    Ward = new WardResponseDto
                    {
                        Id = a.WardId,
                        Name = a.Ward.Name,
                        Description = a.Ward.Description
                    }
                })],

                BedAssignments = [.. p.BedAssignments.Select(ba => new BedAssignmentResponseDto
                {
                    Id = ba.Id,
                    From = ba.From,
                    To = ba.To,
                    Bed = new BedResponseDto
                    {
                        Id = ba.BedId,
                        BedType = new BedTypeResponseDto
                        {
                            Id = ba.Bed.BedTypeId,
                            Name = ba.Bed.BedType.Name,
                            Description = ba.Bed.BedType.Description
                        },
                        Room = new RoomResponseDto
                        {
                            Id = ba.Bed.RoomId,
                            HasTv = ba.Bed.Room.HasTv,
                            Ward = new WardResponseDto
                            {
                                Id = ba.Bed.Room.WardId,
                                Name = ba.Bed.Room.Ward.Name,
                                Description = ba.Bed.Room.Ward.Description
                            }
                        }
                    }
                })]
            })];
        }
    }
}