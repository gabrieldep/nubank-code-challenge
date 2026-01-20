using System.Text.Json.Serialization;

namespace nubank_code_challenge.DTOs;

public record OperationModel(
    [property: JsonPropertyName("operation")] string Operation,
    [property: JsonPropertyName("unit-cost")] decimal UnitCost,
    [property: JsonPropertyName("quantity")] int Quantity
);