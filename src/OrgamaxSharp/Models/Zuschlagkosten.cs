using System.Xml;
using OrgamaxSharp.Extensions;

namespace OrgamaxSharp.Models;

public record Zuschlagkosten
{
    /// <summary>
    /// Dieses Feld steht für die Netto-Zuschlagskosten1-3 der Bestellung.
    /// Anmerkung: Bei Zuschlagskosten müssen nur entweder Netto- oder Bruttopreise sowie der entsprechende Mehrwertsteuersatz hinterlegt werden.
    /// </summary>
    public decimal? ZuschlagkostenNetto { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für die Brutto-Zuschlagskosten1-3 der Bestellung.
    /// Anmerkung: Bei Zuschlagskosten müssen nur entweder Netto- oder Bruttopreise sowie der entsprechende Mehrwertsteuersatz hinterlegt werden.
    /// </summary>
    public decimal? ZuschlagkostenBrutto { get; init; }
    
    /// <summary>
    /// Dieses Feld steht für den Mehrwertsteuersatz der Zuschlagskosten1-3 der Bestellung.
    /// Anmerkung: Bei Zuschlagskosten müssen nur entweder Netto- oder Bruttopreise sowie der entsprechende Mehrwertsteuersatz hinterlegt werden.
    /// </summary>
    public decimal? ZuschlagkostenMwStProzent { get; init; }
}

public static class ZuschlagkostenExtensions
{
    public static async Task WriteXml(this IReadOnlyCollection<Zuschlagkosten>? Zuschlagkosten, XmlWriter writer)
    {
        await writer.WriteStartElementAsync(null, "Zuschlagkosten", null);
            
        for (int i = 0; i < 3; ++i)
        {
            await writer.Wr($"ZuschlagkostenNetto{i + 1}", Zuschlagkosten?.ElementAtOrDefault(i)?.ZuschlagkostenNetto);
            await writer.Wr($"ZuschlagkostenBrutto{i + 1}", Zuschlagkosten?.ElementAtOrDefault(i)?.ZuschlagkostenNetto);
            await writer.Wr($"ZuschlagkostenMwStProzen{i + 1}", Zuschlagkosten?.ElementAtOrDefault(i)?.ZuschlagkostenNetto);
        }

        await writer.WriteEndElementAsync();
    }
}