using System.Text;
using System.Text.Json;
using nubank_code_challenge.Context;
using nubank_code_challenge.DTOs;

namespace nubank_code_challenge;

public class Program
{
    public static void Main(string[] args)
    {
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        using var reader = Console.In;

        while (reader.Peek() != -1)
        {
            var jsonArray = ReadNextArray(reader);
            
            if (string.IsNullOrWhiteSpace(jsonArray)) continue;

            try
            {
                var operations = JsonSerializer.Deserialize<List<OperationModel>>(jsonArray, jsonOptions);
                if (operations != null)
                {
                    ProcessSimulation(operations);
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private static string? ReadNextArray(TextReader reader)
    {
        var builder = new StringBuilder();
        var openBrackets = 0;
        var started = false;

        while (reader.Peek() != -1)
        {
            var c = (char)reader.Read();
            
            if (c == '[')
            {
                started = true;
                openBrackets++;
            }

            if (started) builder.Append(c);

            if (c == ']')
            {
                openBrackets--;
                if (openBrackets == 0 && started)
                    return builder.ToString();
            }
        }

        return null;
    }

    private static void ProcessSimulation(List<OperationModel> operations)
    {
        var context = new SimulationContext();
        var results = new List<TaxResponse>();

        foreach (var op in operations)
        {
            var tax = context.CalculateOperationTax(op);
            results.Add(new TaxResponse(tax));
        }
        Console.WriteLine(JsonSerializer.Serialize(results));
    }
}