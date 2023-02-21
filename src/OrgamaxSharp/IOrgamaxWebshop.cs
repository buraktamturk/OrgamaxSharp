using OrgamaxSharp.Models;

namespace OrgamaxSharp;

public interface IOrgamaxIntegration
{
    string WebshopName { get; }
    
    Version Version { get; }
    
    Capabilities Capabilities { get; }

    Task<bool> HasOpenOrder();
    
    IAsyncEnumerable<Bestellvorgang> GetOrders();
    
    Task StatusChange(long orderNumber);

    IAsyncEnumerable<Artikel> GetProducts();

    IAsyncEnumerable<ProductListe> GetProductListe();

    Task<int> ImportProducts(IAsyncEnumerable<Artikel> products);
    
    Task<int> ImportStockCounts(IAsyncEnumerable<Stock> stockCounts);

    Task<int> ImportPriceLists(IAsyncEnumerable<ArticelPrice> priceLists);
    
    Task<int> GetProductCount();
}
