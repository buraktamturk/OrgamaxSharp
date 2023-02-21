using System.Xml;
using System.Xml.Linq;

namespace OrgamaxSharp.Models;

public record Stock
{
    public string Artikelnummer { get; init; }
    
    public string ArtikelnummerWebshop { get; init; }
    
    public decimal? LagerBestandAktuell { get; init; }
    
    public decimal? LagerMindestBestand { get; init; }
    
    public decimal? LagerWiederbeschaffungsdauer { get; init; }

    public Stock()
    {
        
    }

    public Stock(XElement elem)
    {
        Artikelnummer = elem.Element("Artikelnummer")?.Value;
        ArtikelnummerWebshop = elem.Element("ArtikelnummerWebshop")?.Value;
        var lagerBestandAktuell = elem.Element("LagerBestandAktuell")?.Value;
        LagerBestandAktuell = string.IsNullOrEmpty(lagerBestandAktuell) || lagerBestandAktuell == "DSC_IGNORE" ? null : decimal.Parse(lagerBestandAktuell);
        var lagerMindestBestand = elem.Element("LagerMindestBestand")?.Value;
        LagerMindestBestand = string.IsNullOrEmpty(lagerMindestBestand) || lagerMindestBestand == "DSC_IGNORE" ? null : decimal.Parse(lagerMindestBestand);
        var lagerWiederbeschaffungsdauer = elem.Element("LagerWiederbeschaffungsdauer")?.Value;
        LagerWiederbeschaffungsdauer = string.IsNullOrEmpty(lagerWiederbeschaffungsdauer) || lagerWiederbeschaffungsdauer == "DSC_IGNORE" ? null : decimal.Parse(lagerWiederbeschaffungsdauer);
    }
}