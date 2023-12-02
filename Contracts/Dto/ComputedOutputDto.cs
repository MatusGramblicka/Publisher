using System.Text.Json.Serialization;

namespace Contracts.Dto;

public record ComputedOutputDto
{
    [JsonPropertyName("computed_value")]
    public double? ComputedValue { get; set; }
    [JsonPropertyName("input_value")]
    public decimal InputValue { get; set; }
    [JsonPropertyName("previous_value")]
    public double? PreviousValue { get; set; }

}