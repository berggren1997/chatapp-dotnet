using System.Text.Json;

namespace ChatApp.Api.Models.ErrorModel;

public class ErrorDetail
{
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    public override string ToString() => JsonSerializer.Serialize(this);
}
