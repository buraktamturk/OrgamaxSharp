using System.Xml;

namespace OrgamaxSharp.Models;

public record Kundendaten
{
    public Kunde? Kunde { get; init; }
    
    public Kontodaten? Kontodaten { get; init; }
    
    public Abweichend? AbweichendLieferung { get; init; }
    
    public Abweichend? AbweichendRechnung { get; init; }
    
    public Frachtkosten? Frachtkosten { get; init; }
    
    public async Task WriteXml(XmlWriter writer)
    {
        await writer.WriteStartElementAsync(null, "Kundendaten", null);
        await (Kunde ?? new Kunde()).WriteXml(writer);
        await (Kontodaten ?? new Kontodaten()).WriteXml(writer);
        await (AbweichendLieferung ?? new Abweichend()).WriteXml(writer, "abweichendLieferung");
        await (AbweichendRechnung ?? new Abweichend()).WriteXml(writer, "abweichendRechnung");
        await (Frachtkosten ?? new Frachtkosten()).WriteXml(writer, "abweichendRechnung");
        await writer.WriteEndElementAsync();
    }
}