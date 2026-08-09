namespace SmartHire.Application.DTOs.Uploads
{
    public class UploadFileResponse
    {
        public string FileUrl { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
    }
}
