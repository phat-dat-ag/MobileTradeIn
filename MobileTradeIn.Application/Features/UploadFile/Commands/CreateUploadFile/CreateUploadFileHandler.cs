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
            "Business Started. Operation={Operation}. FileName={FileName}. FileType={FileType}",
            "CreateUploadFile",
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

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. UploadFileId={UploadFileId}",
            "CreateUploadFileDatabase",
            result.UploadFileId);

        stopwatch.Stop();

        _logger.LogInformation(
            "Business Completed. Operation={Operation}. UploadFileId={UploadFileId}. Elapsed={ElapsedMilliseconds}ms",
            "CreateUploadFile",
            result.UploadFileId,
            stopwatch.ElapsedMilliseconds);

        return result;
    }
}