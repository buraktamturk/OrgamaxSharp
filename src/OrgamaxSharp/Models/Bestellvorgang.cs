using System.Globalization;
using System.Xml;
using OrgamaxSharp.Extensions;

namespace OrgamaxSharp.Models;

public record Bestellvorgang
{
    /// <summary>
    /// Dieses Feld steht für die Bestellnummer im Shop.
    /// orgaMAX benötigt diese Angabe, um die Bestelldaten importieren zu können und den Status der Bestellung im Shop ändern zu können.
    /// </summary>
    public long BestellnummerShop { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für das Datum der Bestellung.
    /// </summary>
    public DateTimeOffset? Bestelldatum { get; init; }
    
    /// <summary>
    /// Beschreibung In diesem Feld kann ein vom Kunden gewünschtes Lieferdatum eingetragen werden.
    /// </summary>
    public DateTimeOffset? Wunschlieferdatum { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die jeweiligen Lieferbedingungen (z.B. DHL, UPS, FedEX).
    /// Achtung: Um die Lieferbedingungen korrekt übernehmen zu können, müssen diese in den Stammdaten von orgaMAX hinterlegt sein.
    /// </summary>
    public string? Lieferart { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die jeweiligen Zahlungsarten (z.B. Rechnung, Nachnahme, Lastschrift).
    /// Achtung: Um die Zahlungsarten korrekt übernehmen zu können, müssen diese in den Stammdaten von orgaMAX hinterlegt sein.
    /// </summary>
    public string? Zahlungsart { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den Bruttobestellwert.
    /// Achtung: Wenn Zuschlagskosten wie Versandkosten als Bestellpositionen an orgaMAX weitergeführt werden,
    /// sollte dieses Feld leer bleiben, da es sonst zu Problemen im Abgleich der Summen kommen kann.
    /// Die meisten Webshopsysteme geben Versandkosten nicht als Bestellposition an und rechnen diese somit auch nicht in den Bestellwert ein.
    /// </summary>
    public decimal? BestellwertBrutto { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den Bruttobestellwert.
    /// Achtung: Wenn Zuschlagskosten wie Versandkosten als Bestellpositionen an orgaMAX weitergeführt werden,
    /// sollte dieses Feld leer bleiben, da es sonst zu Problemen im Abgleich der Summen kommen kann.
    /// Die meisten Webshopsysteme geben Versandkosten nicht als Bestellposition an und rechnen diese somit auch nicht in den Bestellwert ein.
    /// </summary>
    public IReadOnlyCollection<string>? ZusatzfeldBestellung { get; init; }
    
    public Kundendaten Kundendaten { get; init; }
    
    public IReadOnlyCollection<Zuschlagkosten>? Zuschlagkosten { get; init; }

    /// <summary>
    /// Dieses Feld steht für die Anmerkungen der Bestellung. In vielen Webshopsystemen können die Kunden hier zusätzliche Informationen hinterlegen.
    /// </summary>
    public string? AnmerkungenBestellung { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für das "individuelle Feld 1-5" der Bestellung.
    /// Anmerkung: Die individuellen Felder können mit Hilfe des Vorlagen-Designers auch auf den Ausdrucken hinterlegt werden.
    /// </summary>
    public IReadOnlyCollection<BestellArtikel>? BestellArtikels { get; init; }

    public async Task WriteXml(XmlWriter writer)
    {
        await writer.WriteStartElementAsync(null, "Bestellvorgang", null);

        await writer.Wr("BestellnummerShop", BestellnummerShop);
        await writer.Wr("Bestelldatum", Bestelldatum);
        await writer.Wr("Wunschlieferdatum", Wunschlieferdatum);
        await writer.Wr("Lieferart", Lieferart);
        await writer.Wr("Zahlungsart", Zahlungsart);
        await writer.Wr("BestellwertBrutto", BestellwertBrutto);
        for (int i = 0; i < 5; ++i)
        {
            await writer.Wr($"ZusatzfeldBestellung{i+1}", ZusatzfeldBestellung?.ElementAtOrDefault(i));
        }
        
        await Kundendaten.WriteXml(writer);
        await Zuschlagkosten.WriteXml(writer);

        if (BestellArtikels is not null) {
            foreach (var bestellArtikel in BestellArtikels)
            {
                await bestellArtikel.WriteXml(writer);
            }
        }
        
        await writer.Wr("AnmerkungenBestellung", AnmerkungenBestellung);

        await writer.WriteEndElementAsync();
    }
}