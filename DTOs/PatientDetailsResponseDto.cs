namespace APBD_cwiczenia6.DTOs
{
    public class PatientDetailsResponseDto
    {
        public string Pesel { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int Age { get; set; }

        public string Sex { get; set; } = null!;

        public List<AdmissionResponseDto> Admissions { get; set; } = [];

        public List<BedAssignmentResponseDto> BedAssignments { get; set; } = [];
    }
}