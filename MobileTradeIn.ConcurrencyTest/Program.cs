using System.Net.Http.Json;

var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

var client = new HttpClient(handler);

client.BaseAddress = new Uri("https://localhost:7201");

var tasks = Enumerable.Range(1, 20)
    .Select(async i =>
    {
        var response = await client.PostAsJsonAsync(
            "/api/tradein/confirm",
            new
            {
                tradeInOfferId = 6,
                confirmedBy = $"USER-{i}",
                notes = "Concurrency Test"
            });

        Console.WriteLine(
            $"Request {i}: {(int)response.StatusCode}");

        Console.WriteLine(
            await response.Content.ReadAsStringAsync());
    });

await Task.WhenAll(tasks);

Console.WriteLine();
Console.WriteLine("Finished.");
Console.ReadLine();