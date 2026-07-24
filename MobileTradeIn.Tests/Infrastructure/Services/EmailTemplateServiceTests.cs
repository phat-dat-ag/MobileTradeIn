using MobileTradeIn.Infrastructure.Services;

namespace MobileTradeIn.Tests.Infrastructure.Services;

public class EmailTemplateServiceTests
{
    private readonly EmailTemplateService _service = new();

    [Fact]
    public void RenderContentFromEmailTemplate_ShouldReplaceSinglePlaceholder()
    {
        var template = "Hello {{Name}}";
        var values = new Dictionary<string, string>
        {
            { "Name", "John" }
        };

        var result = _service.RenderContentFromEmailTemplate(template, values);

        Assert.Equal("Hello John", result);
    }

    [Fact]
    public void RenderContentFromEmailTemplate_ShouldReplaceMultiplePlaceholders()
    {
        var template = "Hello {{Name}}, your order {{OrderId}} is confirmed.";
        var values = new Dictionary<string, string>
        {
            { "Name", "John" },
            { "OrderId", "1001" }
        };

        var result = _service.RenderContentFromEmailTemplate(template, values);

        Assert.Equal("Hello John, your order 1001 is confirmed.", result);
    }

    [Fact]
    public void RenderContentFromEmailTemplate_ShouldLeaveUnknownPlaceholderUnchanged()
    {
        var template = "Hello {{Name}}, {{Company}}";
        var values = new Dictionary<string, string>
        {
            { "Name", "John" }
        };

        var result = _service.RenderContentFromEmailTemplate(template, values);

        Assert.Equal("Hello John, {{Company}}", result);
    }

    [Fact]
    public void RenderContentFromEmailTemplate_ShouldReturnOriginalTemplate_WhenValuesAreEmpty()
    {
        var template = "Hello {{Name}}";

        var result = _service.RenderContentFromEmailTemplate(
            template,
            new Dictionary<string, string>());

        Assert.Equal(template, result);
    }

    [Fact]
    public void RenderContentFromEmailTemplate_ShouldReturnEmptyString_WhenTemplateIsEmpty()
    {
        var result = _service.RenderContentFromEmailTemplate(
            string.Empty,
            new Dictionary<string, string>
            {
                { "Name", "John" }
            });

        Assert.Equal(string.Empty, result);
    }
}