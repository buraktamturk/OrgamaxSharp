using System.Xml;
using OrgamaxSharp.Extensions;

namespace OrgamaxSharp.Models;

// abweichendRechnung
// abweichendLieferung
public record Abweichend
{
    public string Firmenname { get; init; }
    
    public string Firmenzusatz { get; init; }
    
    public string PersAnrede { get; init; }
    
    public string PersGeschl { get; init; }
    
    public string PersTitel { get; init; }
    
    public string PersNachname { get; init; }

    public string PersVorname { get; set; }

    public string Strasse { get; set; }

    public string Postleitzahl { get; set; }
    
    public string Ort { get; set; }
    
    public string Laendercode { get; set; }
    
    public string Land { get; init; }
    
    public string Email { get; init; }
    
    public string Telefon { get; init; }
    
    public string Fax { get; init; }

    public async Task WriteXml(XmlWriter writer, string tag)
    {
        await writer.WriteStartElementAsync(null, string.Concat(tag[0].ToString().ToUpper(), tag.AsSpan(1)), null);

        await writer.Wr($"{tag}Firmenname", Firmenname);
        await writer.Wr($"{tag}Firmenzusatz", Firmenzusatz);
        await writer.Wr($"{tag}PersAnrede", PersAnrede);
        await writer.Wr($"{tag}PersGeschl", PersGeschl);
        await writer.Wr($"{tag}PersTitel", PersTitel);
        await writer.Wr($"{tag}PersNachname", PersNachname);
        await writer.Wr($"{tag}PersVorname", PersVorname);
        await writer.Wr($"{tag}Strasse", Strasse);
        await writer.Wr($"{tag}Postleitzahl", Postleitzahl);
        await writer.Wr($"{tag}Ort", Ort);
        await writer.Wr($"{tag}Laendercode", Laendercode);
        await writer.Wr($"{tag}Land", Land);
        await writer.Wr($"{tag}Email", Email);
        await writer.Wr($"{tag}Telefon", Telefon);
        await writer.Wr($"{tag}Fax", Fax);
        
        await writer.WriteEndElementAsync();
    }
}