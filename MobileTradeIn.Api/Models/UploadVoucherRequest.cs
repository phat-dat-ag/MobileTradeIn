using System.ComponentModel.DataAnnotations;

namespace MobileTradeIn.Api.Models;

public class UploadVoucherRequest
{
    [Required]
    public IFormFile File { get; set; } = default!;

    [Required]
    public string UploadedBy { get; set; } = string.Empty;
}