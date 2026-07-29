using MobileTradeIn.Application.Common.Exceptions.Business;
using System.Text.RegularExpressions;

public class FileValidator : IFileValidator
{
    public void ValidateFileName(string fileName)
    {
        if (fileName.Length == 0)
            throw new BusinessException("File's name is empty.");

        if (!Regex.IsMatch(
            fileName,
            @"^Voucher_[0-9]{8}\.csv$",
            RegexOptions.IgnoreCase))
        {
            throw new BusinessException(
                "Invalid file name.");
        }
    }
}