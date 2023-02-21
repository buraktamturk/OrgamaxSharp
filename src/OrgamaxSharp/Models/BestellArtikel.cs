using System.Globalization;
using System.Xml;
using OrgamaxSharp.Extensions;

namespace OrgamaxSharp.Models;

public record BestellArtikel
{
    /// <summary>
    /// Dieses Feld steht für die Positionsnummer des bestellten Artikels.
    /// </summary>
    public int? Positionsnummer { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Artikelnummer des bestellten Artikels in orgaMAX.
    /// Manche Shopsysteme bieten die Möglichkeit sowohl eine Shopartikelnummer als auch eine Warenwirtschaftsartikelnummer zu hinterlegen.
    /// Anmerkung: Wenn die Artikelnummer des Webshops mit der Artikelnummer in orgaMAX übereinstimmt, kann man diese auch direkt als Artikelnummer an orgaMAX übergeben.
    /// </summary>
    public string Artikelnummer { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Artikelnummer des bestellten Artikels im Webshop. Anmerkung:
    /// Die Artikelnummer des Webshops kann in den Artikelstammdaten von orgaMAX unter "Sonstiges" hinterlegt werden.
    /// </summary>
    public string ArtikelnummerShop { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Menge (oder Anzahl) des bestellten Artikels.
    /// </summary>
    public decimal? Menge { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den abweichenden Nettopreis des bestellten Artikels.
    /// Relevant wird dieses Feld zum Beispiel, wenn im Webshop andere Preise gelten als in orgaMAX hinterlegt (z.B. bei Aktionen).
    /// Anmerkung: Diese Angabe wird nur im Vorgang, nicht in den Stammdaten gespeichert.
    /// </summary>
    public decimal? abweichenderEinzelpreisNetto { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den abweichenden Bruttopreis des bestellten Artikels.
    /// Relevant wird dieses Feld zum Beispiel, wenn im Webshop andere Artikeltexte hinterlegt sind als in orgaMAX.
    /// Anmerkung: Diese Angabe wird nur im Vorgang, nicht in den Stammdaten gespeichert.
    /// </summary>
    public decimal? abweichenderEinzelpreisBrutto { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den abweichenden Mehrwertsteuersatz des bestellten Artikels.
    /// Relevant wird dieses Feld um Beispiel, wenn im Webshop andere Mehrwertsteuersätze gelten als in orgaMAX hinterlegt (z.B. für Nettorechnungen bei vorhandener Umsatzsteueridentnummer).
    /// Anmerkung: Diese Angabe wird nur im Vorgang, nicht in den Stammdaten gespeichert.
    /// </summary>
    public decimal? abweichendeMwStProzent { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den abweichenden Artikeltext des bestellten Artikels.
    /// Relevant wird dieses Feld zum Beispiel, wenn im Webshop andere Artikeltexte hinterlegt sind als in orgaMAX.
    /// Anmerkung: Diese Angabe wird nur im Vorgang, nicht in den Stammdaten gespeichert.
    /// </summary>
    public string abweichenderArtikeltext { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für gewährten Rabatt auf den bestellten Artikel in Prozent (z.B. für Aktionen).
    /// </summary>
    public decimal? RabattProzent { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für das "individuelle Feld 1-5" des bestellten Artikels (z.B. für Artikelattribute oder Artikelvarianten).
    /// Anmerkung: Die individuellen Felder können mit Hilfe des Vorlagen Designers auch auf den Ausdrucken hinterlegt werden.
    /// </summary>
    public IReadOnlyCollection<string>? ZusatzfeldPosition { get; init; }

    public async Task WriteXml(XmlWriter writer)
    {
        await writer.WriteStartElementAsync(null, "BestellArtikel", null);

        await writer.Wr("Positionsnummer", Positionsnummer);
        await writer.Wr("Artikelnummer", Artikelnummer);
        await writer.Wr("ArtikelnummerShop", ArtikelnummerShop);
        await writer.Wr("Menge", Menge);
        await writer.Wr("abweichenderEinzelpreisNetto", abweichenderEinzelpreisNetto);
        await writer.Wr("abweichenderEinzelpreisBrutto", abweichenderEinzelpreisBrutto);
        await writer.Wr("abweichendeMwStProzent", abweichendeMwStProzent);
        await writer.Wr("abweichenderArtikeltext", abweichenderArtikeltext);
        await writer.Wr("RabattProzent", RabattProzent);
        for (int i = 0; i < 5; ++i)
        {
            await writer.Wr($"ZusatzfeldPosition{i+1}", ZusatzfeldPosition?.ElementAtOrDefault(i));
        }

        await writer.WriteEndElementAsync();
    }
}