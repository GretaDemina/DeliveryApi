namespace DeliveryApi.Models;

/// <summary>Город справочника с географическими координатами.</summary>
public record City(string Name, double Latitude, double Longitude);
