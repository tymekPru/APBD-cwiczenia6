using APBD_cwiczenia6.DTOs;
using APBD_cwiczenia6.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_cwiczenia6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController(IPatientService patientService) : ControllerBase
    {
        private readonly IPatientService _patientService = patientService;

        [HttpGet]
        public async Task<ActionResult<List<PatientDetailsResponseDto>>> GetPatients([FromQuery] string? search)
        {
            return Ok(await _patientService.GetAllPatientsAsync(search));
        }

        [HttpPost]
        [Route("{pesel}/bedassignments")]
        public async Task<ActionResult<BedAssignmentResponseDto>> AssignBedToPatient([FromRoute] string pesel, [FromBody] BedAssignmentRequestDto data)
        {
            if (string.IsNullOrWhiteSpace(pesel))
                return NotFound();

            if (data.From > data.To)
                BadRequest("'from' date cannot be greater than 'to'");

            var result = await _patientService.TryAssignBedToPatientAsync(pesel, data);
            return CreatedAtAction(nameof(AssignBedToPatient), new { result.Id }, result);
        }
    }
}