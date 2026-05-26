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
        public async Task<ActionResult<List<PatientDetailsResponseDto>>> GetAll([FromQuery] string? search)
        {
            return Ok(await _patientService.GetAllPatientsAsync(search));
        }
    }
}