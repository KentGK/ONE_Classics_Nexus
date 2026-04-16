using ONE_Classics_Nexus.Domain.Valuation.Models;

namespace ONE_Classics_Nexus.Domain.Valuation.Abstractions;

public interface ISimilarityEngine
{
    decimal CalculateSimilarity0To1(SubjectVehicle subject, ComparableSale comparable);
}