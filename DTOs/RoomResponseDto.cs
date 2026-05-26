namespace APBD_cwiczenia6.DTOs
{
    public class RoomResponseDto
    {
        public string Id { get; set; } = null!;

        public bool HasTv { get; set; }

        public virtual WardResponseDto Ward { get; set; } = null!;
    }
}