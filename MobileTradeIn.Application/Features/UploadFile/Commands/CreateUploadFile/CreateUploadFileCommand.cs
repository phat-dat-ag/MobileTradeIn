using MediatR;
using MobileTradeIn.Application.DTOs.UploadFile;
using System.ComponentModel.DataAnnotations;

namespace MobileTradeIn.Application.Features.UploadFile.Commands.CreateUploadFile;

public class CreateUploadFileCommand
    : IRequest<CreateUploadFileResponse>
{
    [Required]
    public string FileName { get; set; } = string.Empty;

    [Required]
    public string FilePath { get; set; } = string.Empty;

    [Required]
    public string FileType { get; set; } = string.Empty;

    [Required]
    public string UploadedBy { get; set; } = string.Empty;
}