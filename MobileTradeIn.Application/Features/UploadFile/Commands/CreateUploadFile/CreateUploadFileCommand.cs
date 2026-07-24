using MediatR;
using MobileTradeIn.Application.DTOs.UploadFile;

namespace MobileTradeIn.Application.Features.UploadFile.Commands.CreateUploadFile;

public class CreateUploadFileCommand
    : IRequest<CreateUploadFileResponse>
{
    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string FileType { get; set; } = string.Empty;

    public string UploadedBy { get; set; } = string.Empty;
}