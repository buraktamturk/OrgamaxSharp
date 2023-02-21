using System.Globalization;
using System.Xml;
using OrgamaxSharp.Extensions;

namespace OrgamaxSharp.Models;

public record Frachtkosten
{
    /// <summary>
    /// Dieses Feld steht für die Netto-Frachtkosten der Bestellung.
    /// </summary>
    public decimal? FrachtkostenNetto { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Brutto-Frachtkosten der Bestellung.
    /// </summary>
    public decimal? FrachtkostenBrutto { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Mehrwertsteuer der Frachtkosten der Bestellung.
    /// </summary>
    public decimal? FrachtkostenMwStProzent { get; init; }

    public async Task WriteXml(XmlWriter writer, string tag)
    {
        await writer.WriteStartElementAsync(null, "Frachtkosten", null);

        await writer.Wr("FrachtkostenNetto", FrachtkostenNetto);
        await writer.Wr("FrachtkostenBrutto", FrachtkostenBrutto);
        await writer.Wr("FrachtkostenMwStProzent", FrachtkostenMwStProzent);

        await writer.WriteEndElementAsync();
    }
}