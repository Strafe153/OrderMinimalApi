using Newtonsoft.Json;

namespace OrderMinimalApi.Models;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = default!;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}