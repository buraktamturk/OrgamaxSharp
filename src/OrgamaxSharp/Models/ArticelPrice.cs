using System.Xml.Linq;

namespace OrgamaxSharp.Models;

public record ArticelPrice
{
    public string ArtikelnummerWebshop { get; init; }
    
    public decimal? ArtikelpreisNetto { get; init; }
    
    public decimal? ArtikelPreisBeziehtSichAufMenge { get; init; }

    public ArticelPrice()
    {
        
    }

    public ArticelPrice(XElement elem)
    {
        ArtikelnummerWebshop = elem.Element("ArtikelnummerWebshop")?.Value;
        var artikelpreisNetto = elem.Element("ArtikelpreisNetto")?.Value;
        ArtikelpreisNetto = string.IsNullOrEmpty(artikelpreisNetto) || artikelpreisNetto == "DSC_IGNORE" ? null : decimal.Parse(artikelpreisNetto);
        var artikelPreisBeziehtSichAufMenge = elem.Element("ArtikelPreisBeziehtSichAufMenge")?.Value;
        ArtikelPreisBeziehtSichAufMenge = string.IsNullOrEmpty(artikelPreisBeziehtSichAufMenge) || artikelPreisBeziehtSichAufMenge == "DSC_IGNORE" ? null : decimal.Parse(artikelPreisBeziehtSichAufMenge);
    }
}