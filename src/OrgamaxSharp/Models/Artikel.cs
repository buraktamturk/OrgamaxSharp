using System.Xml;
using System.Xml.Linq;
using OrgamaxSharp.Extensions;

namespace OrgamaxSharp.Models;

public class Artikel
{
    public string Artikelnummer { get; init; }
    
    public string ArtikelnummerWebshop { get; init; }
    
    public string Artikelbeschreibung { get; init; }
    
    public int? MwStCode { get; init; }
    
    public decimal? MwStValue { get; init; }
    
    public string Einheit { get; init; }
    
    public string? Artikelkategorie { get; init; }
    
    public decimal? Gewicht { get; init; }
    
    public string? Volumen { get; init; }
    
    public string? Anmerkungen { get; init; }
    
    public string ArtikelpreisWaehrung { get; init; }
    
    public decimal? ArtikelpreisNetto { get; init; }
    
    public decimal? ArtikelpreisBrutto { get; init; }
    
    public int? ArtikelPreisBeziehtSichAufMenge { get; init; }
    
    public decimal? Einkaufspreis { get; init; }
    
    public string Artikelbild { get; init; }
    
    public IReadOnlyCollection<string>? IndividuellesFelds { get; init; }
    
    public string Bewirtschaftungsart { get; init; }

    public Artikel()
    {
        
    }

    public Artikel(XElement elem)
    {
        Artikelnummer = elem.Descendants("Artikelnummer").FirstOrDefault()?.Value;
        ArtikelnummerWebshop = elem.Descendants("ArtikelnummerWebshop").FirstOrDefault()?.Value;
        Artikelbeschreibung = elem.Descendants("Artikelbeschreibung").FirstOrDefault()?.Value;
        var mwStCode = elem.Descendants("MwStCode").FirstOrDefault()?.Value;
        MwStCode = string.IsNullOrEmpty(mwStCode) || mwStCode == "DSC_IGNORE" ? null : int.Parse(mwStCode);
        var mwStValue = elem.Descendants("MwStValue").FirstOrDefault()?.Value;
        MwStValue = string.IsNullOrEmpty(mwStValue) || mwStValue == "DSC_IGNORE" ? null : decimal.Parse(mwStValue);
        Einheit = elem.Descendants("Einheit").FirstOrDefault()?.Value;
        Artikelkategorie = elem.Descendants("Artikelkategorie").FirstOrDefault()?.Value;
        var ghewicht = elem.Descendants("Ghewicht").FirstOrDefault()?.Value;
        Gewicht = string.IsNullOrEmpty(ghewicht) || ghewicht == "DSC_IGNORE" ? null : decimal.Parse(ghewicht);
        Volumen = elem.Descendants("Volumen").FirstOrDefault()?.Value;
        Anmerkungen = elem.Descendants("Anmerkungen").FirstOrDefault()?.Value;
        ArtikelpreisWaehrung = elem.Descendants("ArtikelpreisWaehrung").FirstOrDefault()?.Value;
        var artikelpreisNetto = elem.Descendants("ArtikelpreisNetto").FirstOrDefault()?.Value;
        ArtikelpreisNetto = string.IsNullOrEmpty(artikelpreisNetto) || artikelpreisNetto == "DSC_IGNORE" ? null : decimal.Parse(artikelpreisNetto);
        var artikelpreisBrutto = elem.Descendants("ArtikelpreisBrutto").FirstOrDefault()?.Value;
        ArtikelpreisBrutto = string.IsNullOrEmpty(artikelpreisBrutto) || artikelpreisBrutto == "DSC_IGNORE" ? null : decimal.Parse(artikelpreisBrutto);
        var artikelPreisBeziehtSichAufMenge = elem.Descendants("ArtikelPreisBeziehtSichAufMenge").FirstOrDefault()?.Value;
        ArtikelPreisBeziehtSichAufMenge = string.IsNullOrEmpty(artikelPreisBeziehtSichAufMenge) || artikelPreisBeziehtSichAufMenge == "DSC_IGNORE" ? null : int.Parse(artikelPreisBeziehtSichAufMenge);
        var einkaufspreis = elem.Descendants("Einkaufspreis").FirstOrDefault()?.Value;
        Einkaufspreis = string.IsNullOrEmpty(einkaufspreis) || einkaufspreis == "DSC_IGNORE" ? null : decimal.Parse(einkaufspreis);
        Artikelbild = elem.Descendants("Artikelbild").FirstOrDefault()?.Value;
        IndividuellesFelds = elem.Descendants().Where(a => a.Name.ToString().StartsWith("IndividuellesFeld")).Select(x => x.Value).ToList();
        Bewirtschaftungsart = elem.Descendants("Bewirtschaftungsart").FirstOrDefault()?.Value;
    }
    
    public async Task WriteXml(XmlWriter writer)
    {
        await writer.WriteStartElementAsync(null, "row", null);

        await writer.Wr("Artikelnummer", Artikelnummer);
        await writer.Wr("ArtikelnummerWebshop", ArtikelnummerWebshop);
        await writer.Wr("Artikelbeschreibung", Artikelbeschreibung);
        await writer.Wr("MwStCode", MwStCode);
        await writer.Wr("MwStValue", MwStValue);
        await writer.Wr("Einheit", Einheit);
        await writer.Wr("Artikelkategorie", Artikelkategorie);
        await writer.Wr("Ghewicht", Gewicht);
        await writer.Wr("Volumen", Volumen);
        await writer.Wr("Anmerkungen", Anmerkungen);
        await writer.Wr("ArtikelpreisWaehrung", ArtikelpreisWaehrung);
        await writer.Wr("ArtikelpreisNetto", ArtikelpreisNetto);
        await writer.Wr("ArtikelpreisBrutto", ArtikelpreisBrutto);
        await writer.Wr("ArtikelPreisBeziehtSichAufMenge", ArtikelPreisBeziehtSichAufMenge);
        await writer.Wr("Einkaufspreis", Einkaufspreis);
        await writer.Wr("Artikelbild", Artikelbild);
        await writer.Wr("Bewirtschaftungsart", Bewirtschaftungsart);

        for (int i = 0; i < 5; ++i)
        {
            await writer.Wr($"IndividuellesFeld{i+1}", IndividuellesFelds?.ElementAtOrDefault(i));
        }

        await writer.WriteEndElementAsync();
    }
}