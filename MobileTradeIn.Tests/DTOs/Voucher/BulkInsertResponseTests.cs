namespace MobileTradeIn.Tests.Application.DTOs.Voucher;

public class BulkInsertResponseTests
{
    [Fact]
    public void BulkInsertResponse_ShouldSetAndGetProperties()
    {
        var response = new BulkInsertResponse
        {
            TotalInserted = 100
        };

        Assert.Equal(100, response.TotalInserted);
    }
}