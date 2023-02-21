using System.Xml;
using OrgamaxSharp.Extensions;

namespace OrgamaxSharp.Models;

public record ProductListe
{
    public string ArtikelnummerWebshop { get; init; }
    
    public string Artikelbeschreibung { get; init; }
    
    public async Task WriteXml(XmlWriter writer)
    {
        await writer.WriteStartElementAsync(null, "row", null);
        await writer.Wr("ArtikelnummerWebshop", ArtikelnummerWebshop);
        await writer.Wr("Artikelbeschreibung", Artikelbeschreibung);
        await writer.WriteEndElementAsync();
    }
}