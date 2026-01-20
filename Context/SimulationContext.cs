using nubank_code_challenge.DTOs;

namespace nubank_code_challenge.Context;

public class SimulationContext
{
    private decimal _weightedAverage = 0;
    private int _currentQuantity = 0;
    private decimal _accumulatedLoss = 0;
    private const decimal TaxRate = 0.20m;
    private const decimal TaxExemptionLimit = 20000m;

    public decimal CalculateOperationTax(OperationModel op)
    {
        if (!op.Operation.Equals("buy", StringComparison.OrdinalIgnoreCase)) 
            return HandleSell(op);
        HandleBuy(op);
        return 0.00m;
    }

    private void HandleBuy(OperationModel op)
    {
        var totalCost = (_currentQuantity * _weightedAverage) + (op.Quantity * op.UnitCost);
        _currentQuantity += op.Quantity;
        _weightedAverage = _currentQuantity > 0 ? Math.Round(totalCost / _currentQuantity, 2) : 0;
    }

    private decimal HandleSell(OperationModel op)
    {
        var totalOperationValue = op.Quantity * op.UnitCost;
        var totalCostBasis = op.Quantity * _weightedAverage;
        var profitOrLoss = totalOperationValue - totalCostBasis;

        _currentQuantity -= op.Quantity;

        switch (profitOrLoss)
        {
            case < 0:
                _accumulatedLoss += Math.Abs(profitOrLoss);
                return 0.00m;
            case > 0 when totalOperationValue <= TaxExemptionLimit:
                return 0.00m;
            case > 0:
            {
                var taxableProfit = profitOrLoss - _accumulatedLoss;

                if (taxableProfit <= 0)
                {
                    _accumulatedLoss = Math.Abs(taxableProfit);
                    return 0.00m;
                }
                _accumulatedLoss = 0;
                return Math.Round(taxableProfit * TaxRate, 2);
            }
            default:
                return 0.00m;
        }
    }
}