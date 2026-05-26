namespace APBD_cwiczenia6.DTOs
{
    public class AdmissionResponseDto
    {
        public int Id { get; set; }

        public DateTime AdmissionDate { get; set; }

        public DateTime? DischargeDate { get; set; }

        public WardResponseDto Ward { get; set; } = null!;
    }
}