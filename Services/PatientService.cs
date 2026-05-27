using APBD_cwiczenia6.DTOs;
using APBD_cwiczenia6.Exceptions;
using APBD_cwiczenia6.Models;
using APBD_cwiczenia6.Repositories;

namespace APBD_cwiczenia6.Services
{
    public class PatientService(
            IPatientRepository patientRepository,
            IWardRepository wardRepository,
            IBedTypeRepository bedTypeRepository,
            IBedRepository bedRepository,
            IBedAssignmentRepository bedAssignmentRepository
        ) : IPatientService
    {
        private readonly IPatientRepository _patientRepository = patientRepository;
        private readonly IWardRepository _wardRepository = wardRepository;
        private readonly IBedTypeRepository _bedTypeRepository = bedTypeRepository;
        private readonly IBedRepository _bedRepository = bedRepository;
        private readonly IBedAssignmentRepository _bedAssignmentRepository = bedAssignmentRepository;

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
        public async Task<BedAssignmentResponseDto> TryAssignBedToPatientAsync(string pesel, BedAssignmentRequestDto data)
        {
            var patient = await _patientRepository.GetPatientByPeselAsync(pesel) ?? throw new NotFoundException("Patient with given PESEL was not found.");
            var ward = await _wardRepository.GetWardByNameAsync(data.Ward) ?? throw new NotFoundException("Ward with given name was not found.");
            var bedType = await _bedTypeRepository.GetBedTypeByNameAsync(data.BedType) ?? throw new NotFoundException("Bed type with given name was not found.");
            var availableBed = await _bedRepository.GetAvailableBed(data.From, data.To, ward, bedType) ?? throw new NotFoundException("No available bed of the requested type was found in the selected ward and time period.");

            var bedAssignment = new BedAssignment
            {
                PatientPesel = pesel,
                BedId = availableBed.Id,
                From = data.From,
                To = data.To
            };

            await _bedAssignmentRepository.AddAsync(bedAssignment);
            await _bedAssignmentRepository.SaveChangesAsync();

            return new BedAssignmentResponseDto
            {
                Id = bedAssignment.Id,
                From = bedAssignment.From,
                To = bedAssignment.To,
                Bed = new BedResponseDto
                {
                    Id = availableBed.Id,
                    BedType = new BedTypeResponseDto
                    {
                        Id = availableBed.BedTypeId,
                        Name = availableBed.BedType.Name,
                        Description = availableBed.BedType.Description
                    },
                    Room = new RoomResponseDto
                    {
                        Id = availableBed.RoomId,
                        HasTv = availableBed.Room.HasTv,
                        Ward = new WardResponseDto
                        {
                            Id = availableBed.Room.WardId,
                            Name = availableBed.Room.Ward.Name,
                            Description = availableBed.Room.Ward.Description
                        }
                    }
                }
            };
        }
    }
}