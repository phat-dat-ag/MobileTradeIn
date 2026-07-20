using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.UploadFile;
using MobileTradeIn.Application.Interfaces.Repositories;
using System.Diagnostics;

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
            "Starting upload file creation. FileName={FileName}, FileType={FileType}",
            request.FileName,
            request.FileType);

        var stopwatch = Stopwatch.StartNew();

        var result =
            await _repository.CreateUploadFileAsync(
                new CreateUploadFileRequest
                {
                    FileName = request.FileName,
                    FilePath = request.FilePath,
                    FileType = request.FileType,
                    UploadedBy = request.UploadedBy
                });

        stopwatch.Stop();

        _logger.LogInformation(
            "Upload file creation completed in {ElapsedMilliseconds} ms. UploadFileId={UploadFileId}",
            stopwatch.ElapsedMilliseconds,
            result.UploadFileId);

        return result;
    }
}