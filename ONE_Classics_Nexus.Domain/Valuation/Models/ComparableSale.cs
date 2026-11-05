using System;
namespace ONE_Classics_Nexus.Domain.Valuation.Models;

public sealed class ComparableSale
{
    public ComparableSale()
    {
    }

    public ComparableSale(
        Guid comparableId,
        int year,
        string make,
        string model,
        string? trim,
        string? bodyStyle,
        string? engine,
        string? transmission,
        string? drivetrain,
        string? series,
        decimal salePrice,
        DateTime saleDateUtc,
        string source,
        string? url,
        int? mileage,
        string? venue,
        string? notes)
    {
        ComparableId = comparableId;
        Year = year;
        Make = make;
        Model = model;
        Trim = trim;
        BodyStyle = bodyStyle;
        Engine = engine;
        Transmission = transmission;
        Drivetrain = drivetrain;
        Series = series;
        SalePrice = salePrice;
        SaleDateUtc = saleDateUtc;
        Source = source;
        Url = url;
        Mileage = mileage;
        Venue = venue;
        Notes = notes;
    }

    public Guid ComparableId { get; set; }
    public int Year { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? Trim { get; set; }
    public string? BodyStyle { get; set; }
    public string? Engine { get; set; }
    public string? Transmission { get; set; }
    public string? Drivetrain { get; set; }
    public string? Series { get; set; }
    public decimal SalePrice { get; set; }
    public DateTime SaleDateUtc { get; set; }
    public string Source { get; set; } = string.Empty;
    public string? Url { get; set; }
    public int? Mileage { get; set; }
    public string? Venue { get; set; }
    public string? Notes { get; set; }
}