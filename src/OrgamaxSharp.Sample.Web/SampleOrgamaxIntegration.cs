using OrgamaxSharp.Models;

namespace OrgamaxSharp.Sample.Web;

public class SampleOrgamaxIntegration : IOrgamaxIntegration
{
    public string WebshopName => "Sample Webshop";

    public Version Version => new("1.0.1");

    public Capabilities Capabilities => Capabilities.All;

    private readonly ILogger _logger;
    
    public SampleOrgamaxIntegration(ILogger<SampleOrgamaxIntegration> logger)
    {
        this._logger = logger;
    }

    public Task<bool> HasOpenOrder()
    {
        return Task.FromResult(false);
    }

    public async IAsyncEnumerable<Bestellvorgang> GetOrders()
    {
        this._logger.LogInformation("Getting orders..");
        yield return new Bestellvorgang()
        {
            Bestelldatum = new DateTimeOffset(2022, 11, 15, 14, 12, 43, TimeSpan.Zero),
            BestellnummerShop = 90773,
            Lieferart = "Versand",
            Zahlungsart = "PayPal",
            Kundendaten = new Kundendaten()
            {
                AbweichendLieferung = new Abweichend()
                {
                    Postleitzahl = "89081",
                    Strasse = "Im Oeschle 293",
                    PersNachname = "Theiss",
                    PersVorname = "David",
                    Ort = "Ulm",
                    Laendercode = "DE"
                },
                Frachtkosten = new Frachtkosten()
                {
                    FrachtkostenBrutto = 15,
                    FrachtkostenMwStProzent = 7.7m,
                },
                Kunde = new Kunde()
                {
                    PersonNachname = "Theiss",
                    PersonVorname = "David",
                    Strasse = "Im Oeschle 293",
                    Postleitzahl = "89081",
                    Ort = "Ulm",
                    Laendercode = "DE",
                    Email = "Theiss.David11@mail.de",
                    Telefon = "07315165493"
                }
            },
            BestellArtikels = new BestellArtikel[]
            {
                new()
                {
                    Artikelnummer = "82-50043",
                    abweichenderArtikeltext = "Test",
                    Menge = 1,
                    Positionsnummer = 18141,
                    abweichendeMwStProzent = 7.7m,
                    abweichenderEinzelpreisBrutto = 29,
                    ArtikelnummerShop = "82-50043"
                }
            },
        };
    }

    public Task StatusChange(long orderNumber)
    {
        _logger.LogInformation("Status Change by ERP: #{orderNumber}.", orderNumber);
        return Task.CompletedTask;
    }

    public async IAsyncEnumerable<Artikel> GetProducts()
    {
        yield return new Artikel()
        {
            ArtikelnummerWebshop = "002-001-00093",
            Artikelbeschreibung = @"Lorem Ipsum is simply dummy text of the printing and typesetting industry.
Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type
and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting,
remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages,
and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
            MwStCode = 1,
            Artikelkategorie = "Jollein",
            Gewicht = 0,
            ArtikelpreisNetto = 39.90m,
            IndividuellesFelds = new List<string>()
            {
                "Betthimmel Vintage Nougat"
            }
        };
    }

    public async IAsyncEnumerable<ProductListe> GetProductListe()
    {
        yield return new ProductListe()
        {
            ArtikelnummerWebshop = "82-50043",
            Artikelbeschreibung = "Test"
        };
    }

    public async Task<int> ImportProducts(IAsyncEnumerable<Artikel> products)
    {
        var allProducts = await products.ToListAsync();
        _logger.LogInformation("Importing {count} products..", allProducts.Count);
        return allProducts.Count;
    }

    public async Task<int> ImportStockCounts(IAsyncEnumerable<Stock> stockCounts)
    {
        var allStockCounts = await stockCounts.ToListAsync();
        _logger.LogInformation("Importing {count} stock counts..", allStockCounts.Count);
        return allStockCounts.Count;
    }

    public async Task<int> ImportPriceLists(IAsyncEnumerable<ArticelPrice> priceLists)
    {
        var allPriceLists = await priceLists.ToListAsync();
        _logger.LogInformation("Importing {count} price lists..", allPriceLists.Count);
        return allPriceLists.Count;
    }

    public async Task<int> GetProductCount()
    {
        return 1;
    }
}