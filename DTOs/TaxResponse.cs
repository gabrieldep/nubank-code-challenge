using System.Text.Json.Serialization;

namespace nubank_code_challenge.DTOs;

public record TaxResponse(
    [property: JsonPropertyName("tax")] decimal Tax
);