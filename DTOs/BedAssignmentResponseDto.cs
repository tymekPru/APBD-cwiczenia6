namespace APBD_cwiczenia6.DTOs
{
    public class BedAssignmentResponseDto
    {
        public int Id { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }

        public BedResponseDto Bed { get; set; } = null!;
    }
}