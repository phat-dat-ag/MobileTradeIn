namespace MobileTradeIn.Application.DTOs.UploadFile;

public class CreateUploadFileRequest
{
    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string FileType { get; set; } = string.Empty;

    public string UploadedBy { get; set; } = string.Empty;
}