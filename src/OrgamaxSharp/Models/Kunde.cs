using System.Xml;
using OrgamaxSharp.Extensions;

namespace OrgamaxSharp.Models;

public record Kunde
{
    /// <summary>
    /// Dieses Feld steht für die KundenNr.in orgaMAX. In manchen Webshopsystemen hat man die Möglichkeit, auch die KundenNr. der Warenwirtschaft zu hinterlegen.
    /// Achtung: Wenn in diesem Feld eine KundenNr. steht, weist orgaMAX diese Nummer dem Kunden zu, auch wenn eine andere Nummernfolge Standard ist.
    /// </summary>
    public long? Kundennummer { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Kundennummer im Webshop. Werte, die in diesem Feld stehen, werden von orgaMAX bei der Kundennummervergabe (falls Neukunde) nicht berücksichtigt.
    /// </summary>
    public long? KundennummerWebshop { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den Firmennamen des Kunden.
    /// </summary>
    public string Firmenname { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den Firmenzusatznamen des Kunden.
    /// </summary>
    public string Firmenzusatzname { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Anrede des Kunden.
    /// </summary>
    public string PersonAnrede { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für das Geschlecht des Kunden.
    /// </summary>
    public string PersonGeschlecht { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den Titel des Kunden.
    /// </summary>
    public string PersonTitel { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den Nachnamen des Kunden.
    /// </summary>
    public string PersonNachname { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den Vornamen des Kunden.
    /// </summary>
    public string PersonVorname { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für Straßennamen und Hausnummer der Anschrift des Kunden.
    /// </summary>
    public string Strasse { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Postleitzahl der Anschrift des Kunden.
    /// </summary>
    public string Postleitzahl { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den Stadtnamen der Anschrift des Kunden.
    /// </summary>
    public string Ort { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für das Länderkürzel der Anschrift des Kunden.
    /// </summary>
    public string Laendercode { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für das Land der Anschrift des Kunden.
    /// </summary>
    public string Land { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Emailadresse des Kunden.
    /// </summary>
    public string Email { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Telefonnummer des Kunden.
    /// </summary>
    public string Telefon { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Faxnummer des Kunden.
    /// </summary>
    public string Fax { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Umsatzsteueridentnummer des Kunden.
    /// </summary>
    public string Umsatzsteueridentnummer { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für das "individuelle Feld 1-5" des Kunden.
    /// Anmerkung: Die individuellen Felder können mit Hilfe des Vorlagen Designers auch auf den Ausdrucken hinterlegt werden.
    /// </summary>
    public IReadOnlyCollection<string>? ZusatzfeldKunde { get; init; }
    
    public async Task WriteXml(XmlWriter writer)
    {
        await writer.WriteStartElementAsync(null, "Kunde", null);

        await writer.Wr("Kundennummer", Kundennummer);
        await writer.Wr("KundennummerWebshop", Kundennummer);
        await writer.Wr("Firmenname", Firmenname);
        await writer.Wr("Firmenzusatzname", Firmenzusatzname);
        await writer.Wr("PersonAnrede", PersonAnrede);
        await writer.Wr("PersonGeschlecht", PersonGeschlecht);
        await writer.Wr("PersonTitel", PersonTitel);
        await writer.Wr("PersonNachname", PersonNachname);
        await writer.Wr("PersonVorname", PersonVorname);
        await writer.Wr("Strasse", Strasse);
        await writer.Wr("Postleitzahl", Postleitzahl);
        await writer.Wr("Ort", Ort);
        await writer.Wr("Laendercode", Laendercode);
        await writer.Wr("Land", Land);
        await writer.Wr("Email", Email);
        await writer.Wr("Telefon", Telefon);
        await writer.Wr("Fax", Fax);
        await writer.Wr("Umsatzsteueridentnummer", Umsatzsteueridentnummer);
        
        for (var i = 0; i < 5; i++)
        {
            await writer.Wr($"ZusatzfeldKunde{i + 1}", ZusatzfeldKunde?.ElementAtOrDefault(i));
        }

        await writer.WriteEndElementAsync();
    }
}