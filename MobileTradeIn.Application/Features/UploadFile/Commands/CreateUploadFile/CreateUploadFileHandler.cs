using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.UploadFile;
using MobileTradeIn.Application.Interfaces.Repositories;

namespace MobileTradeIn.Application.Features.UploadFile.Commands.CreateUploadFile;

public class CreateUploadFileHandler
    : IRequestHandler<CreateUploadFileCommand, CreateUploadFileResponse>
{
    private readonly IVoucherRepository _repository;
    private readonly ILogger<CreateUploadFileHandler> _logger;

    public CreateUploadFileHandler(
        IVoucherRepository repository,
        ILogger<CreateUploadFileHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CreateUploadFileResponse> Handle(
        CreateUploadFileCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating upload file. File={FileName}",
            request.FileName);

        var result =
            await _repository.CreateUploadFileAsync(
                new CreateUploadFileRequest
                {
                    FileName = request.FileName,
                    FilePath = request.FilePath,
                    FileType = request.FileType,
                    UploadedBy = request.UploadedBy
                });

        _logger.LogInformation(
            "UploadFile created successfully. Id={UploadFileId}",
            result.UploadFileId);

        return result;
    }
}