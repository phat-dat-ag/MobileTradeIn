using System.ComponentModel.DataAnnotations;

namespace MobileTradeIn.Api.Models;

public class UploadVoucherRequest
{
    [Required]
    public IFormFile File { get; set; } = default!;

    [Required]
    public int UploadFileId { get; set; }

    [Required]
    public string UploadedBy { get; set; } = string.Empty;
}