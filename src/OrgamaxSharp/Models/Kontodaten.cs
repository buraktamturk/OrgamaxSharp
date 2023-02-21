using System.Xml;
using OrgamaxSharp.Extensions;

namespace OrgamaxSharp.Models;

public record Kontodaten
{
    /// <summary>
    /// Dieses Feld steht für den Inhaber des angegebenen Bankkontos.
    /// Achtung: Bei den Bankdaten müssen alle Felder (Inhaber, Kontonummer, Bankleitzahl sowie Bankname) übergeben werden, um diese korrekt in orgaMAX importieren zu können.
    /// </summary>
    public string? BankkontoInhaber { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Kundennummer des angegebenen Bankkontos.
    /// Achtung: Bei den Bankdaten müssen alle Felder (Inhaber, Kontonummer, Bankleitzahl sowie Bankname) übergeben werden, um diese korrekt in orgaMAX importieren zu können.
    /// </summary>
    public string? Bankkontonummer { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Bankleitzahl des angegebenen Bankkontos.
    /// Achtung: Bei den Bankdaten müssen alle Felder (Inhaber, Kontonummer, Bankleitzahl sowie Bankname) übergeben werden, um diese korrekt in orgaMAX importieren zu können.
    /// </summary>
    public string? BankkontoBLZ { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den Banknamen des angegebenen Bankkontos.
    /// Achtung: Bei den Bankdaten müssen alle Felder (Inhaber, Kontonummer, Bankleitzahl sowie Bankname) übergeben werden, um diese korrekt in orgaMAX importieren zu können.
    /// </summary>
    public string? BankkontoBankName { get; init; }
    
    public string? BankkontoIBAN { get; init; }
    
    public string? BankkontoBIC { get; init; }

    public async Task WriteXml(XmlWriter writer)
    {
        await writer.WriteStartElementAsync(null, "Kontodaten", null);

        await writer.Wr("BankkontoInhaber", BankkontoInhaber);
        await writer.Wr("Bankkontonummer", Bankkontonummer);
        await writer.Wr("BankkontoBLZ", BankkontoBLZ);
        await writer.Wr("BankkontoBankName", BankkontoBankName);
        await writer.Wr("BankkontoIBAN", BankkontoIBAN);
        await writer.Wr("BankkontoBIC", BankkontoBIC);

        await writer.WriteEndElementAsync();
    }
}