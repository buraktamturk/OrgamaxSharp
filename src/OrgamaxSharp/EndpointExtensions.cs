using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrgamaxSharp.Models;

namespace OrgamaxSharp;

public static class EndpointExtensions
{
    internal static XmlWriterSettings _settings = new()
    {
        Encoding = new UTF8Encoding(false),
        Async = true,
        Indent = true,
        IndentChars = "  "
    };
    
    private static XmlReaderSettings _readerSettings = new()
    {
        Async = true
    };
    
    public static IEndpointRouteBuilder UseOrgamax<T>(this IEndpointRouteBuilder app, string username, string password, string basePath = "/orgamax")
        where T : IOrgamaxIntegration
    {
        var group = app.MapGroup(basePath)
            .ExcludeFromDescription()
            .AddEndpointFilter(async (invocationContext, next) =>
            {
                if (invocationContext.HttpContext.Request.Headers.UserAgent != username)
                    return Results.Unauthorized();
                
                return await next(invocationContext);
            });
            
        group.MapPost("orgamax_osc_info.php", (IServiceProvider prov) =>
        {
            var service = ActivatorUtilities.CreateInstance<T>(prov);
            
            return Results.Json(new
            {
                osc_version = "22.08.31",
                php_version = "7.4.33",
                openssl_available = true,
                mysqli_available = false,
                shop_system = "individuell",
                mysql_version = (string?)null,
                mysql_error = (string?)null,
                custom = new {
                    service.WebshopName,
                    service.Version
                }
            });
        });

        var valHandler =
            static (IServiceProvider prov, [FromQuery] string function_name, [FromQuery] string CallName) =>
            {
                var service = ActivatorUtilities.CreateInstance<T>(prov);

                return CallName switch
                {
                    "check_function" => Results.Text("1"),
                    "check_version_scf" => Results.Text(service.Version.ToString()),
                    "check_version" => Results.Text("22.08.31"),
                    _ => OrgamaxResults.Error($"Invalid Function Name: {function_name}")
                };
            };
        
        group.MapGet("orgamax_osc_val.php", valHandler);
        group.MapPost("orgamax_osc_val.php", valHandler);
        
        group.MapGet("orgamax_osc.php", (IServiceProvider prov, CancellationToken token) =>
        {
            var service = ActivatorUtilities.CreateInstance<T>(prov);

            return Results.Stream(async stream =>
            {
                using var aes = Aes.Create();
                aes.Key = Encoding.ASCII.GetBytes(password.PadRight(16, '\0'));
                aes.IV = "jxpPjWLPIlLXWxrc"u8.ToArray();
                aes.Padding = PaddingMode.Zeros;
                aes.Mode = CipherMode.CBC;

                using var enc = aes.CreateEncryptor();

                await using var base64Stream =
                    new CryptoStream(stream, new ToBase64Transform(), CryptoStreamMode.Write);
                await using var cryptoStream = new CryptoStream(base64Stream, enc, CryptoStreamMode.Write);
                await using var xmlWriter = XmlWriter.Create(cryptoStream, _settings);

                await xmlWriter.WriteStartDocumentAsync(true);
                await xmlWriter.WriteStartElementAsync(null, "OrderNotification", null);
                await foreach (var order in service.GetOrders().WithCancellation(token))
                {
                    await order.WriteXml(xmlWriter);
                }
                await xmlWriter.WriteEndElementAsync();
                await xmlWriter.WriteEndDocumentAsync();

                await xmlWriter.FlushAsync();
                await cryptoStream.FlushFinalBlockAsync();
                //await base64Stream.FlushFinalBlockAsync();
            }, "text/xml; charset=utf-8");
        });

        var handleOmxToShop = static async (T service, Stream input) =>
        {
            int wholeCount = 0;
            async IAsyncEnumerable<Artikel> Read()
            {
                using var xmlReader = XmlReader.Create(input, _readerSettings);
                while (await xmlReader.ReadAsync())
                {
                    if (!xmlReader.IsStartElement("row"))
                        continue;
                    
                    var elem = await XElement.LoadAsync(xmlReader.ReadSubtree(), LoadOptions.None, default);
                    ++wholeCount;
                    yield return new Artikel(elem);
                }
            }

            var count = await service.ImportProducts(Read());
            return OrgamaxResults.Imported(wholeCount, count);
        };
        
        var handleStockValueToShop = static async (T service, Stream input) =>
        {
            int wholeCount = 0;
            async IAsyncEnumerable<Stock> Read()
            {
                using var xmlReader = XmlReader.Create(input, _readerSettings);
                while (await xmlReader.ReadAsync())
                {
                    if (!xmlReader.IsStartElement("Artikel"))
                        continue;
                    
                    var elem = await XElement.LoadAsync(xmlReader.ReadSubtree(), LoadOptions.None, default);
                    ++wholeCount;
                    yield return new Stock(elem);
                }
            }

            var count = await service.ImportStockCounts(Read());
            return OrgamaxResults.Imported(wholeCount, count);
        };
        
        var handlePriceListToShop = static async (T service, Stream input) =>
        {
            int wholeCount = 0;
            async IAsyncEnumerable<ArticelPrice> Read()
            {
                using var xmlReader = XmlReader.Create(input, _readerSettings);
                while (await xmlReader.ReadAsync())
                {
                    if (!xmlReader.IsStartElement("row"))
                        continue;
                    
                    var elem = await XElement.LoadAsync(xmlReader.ReadSubtree(), LoadOptions.None, default);
                    ++wholeCount;
                    yield return new ArticelPrice(elem);
                }
            }

            var count = await service.ImportPriceLists(Read());
            return OrgamaxResults.Imported(wholeCount, count);
        };

        var handleArticleListFromShop = static (T service, CancellationToken token) =>
        {
            return Results.Stream(async stream =>
            {
                await using var xmlWriter = XmlWriter.Create(stream, _settings);

                await xmlWriter.WriteStartDocumentAsync(true);
                await xmlWriter.WriteStartElementAsync(null, "ArtikelListeWebshop", null);
                await foreach (var product in service.GetProductListe().WithCancellation(token))
                {
                    await product.WriteXml(xmlWriter);
                }

                await xmlWriter.WriteEndElementAsync();
                await xmlWriter.WriteEndDocumentAsync();

                await xmlWriter.FlushAsync();
            }, "text/xml; charset=utf-8");
        };
        
        var handleCheckOpenOrders = static async (T service) =>
        {
            var hasOpenOrder = await service.HasOpenOrder();
            return Results.Text(hasOpenOrder ? "1" : "0");
        };
        
        var handlePagingInformation = static async (T service) =>
        {
            var productCount = await service.GetProductCount();
            return OrgamaxResults.PagingInformation(productCount);
        };
        
        var handleShopToOmx = async (T service, CancellationToken token) =>
        {
            return Results.Stream(async stream =>
            {
                using var aes = Aes.Create();
                aes.Key = Encoding.ASCII.GetBytes(password.PadRight(16, '\0'));
                aes.IV = "jxpPjWLPIlLXWxrc"u8.ToArray();
                aes.Padding = PaddingMode.Zeros;
                aes.Mode = CipherMode.CBC;

                using var enc = aes.CreateEncryptor();

                await using var base64Stream =
                    new CryptoStream(stream, new ToBase64Transform(), CryptoStreamMode.Write);
                await using var cryptoStream = new CryptoStream(base64Stream, enc, CryptoStreamMode.Write);
                await using var xmlWriter = XmlWriter.Create(cryptoStream, _settings);

                await xmlWriter.WriteStartDocumentAsync(true);
                await xmlWriter.WriteStartElementAsync(null, "Artikelimport", null);
                await foreach (var product in service.GetProducts().WithCancellation(token))
                {
                    await product.WriteXml(xmlWriter);
                }

                await xmlWriter.WriteEndElementAsync();
                await xmlWriter.WriteEndDocumentAsync();

                await xmlWriter.FlushAsync();
            }, "text/xml; charset=utf-8");
        };

        var handleSync = async (IServiceProvider prov, [FromQuery] string sync, Stream body, CancellationToken token) =>
        {
            var service = ActivatorUtilities.CreateInstance<T>(prov);

            try
            {
                return sync switch
                {
                    "shop_to_omx" => await handleShopToOmx(service, token),
                    "articlelist_from_shop" => handleArticleListFromShop(service, token),
                    "omx_to_shop" => await handleOmxToShop(service, body),
                    "check_open_orders" => await handleCheckOpenOrders(service),
                    "stockvalue_to_shop" => await handleStockValueToShop(service, body),
                    "pricelist_to_shop" => await handlePriceListToShop(service, body),
                    "paging_informationen" => await handlePagingInformation(service),
                    _ => OrgamaxResults.Error($"Invalid Sync Type: {sync}")
                };
            }
            catch (Exception e)
            {
                return OrgamaxResults.Error(e);
            }
        };
        
        group.MapGet("orgamax_osc_sync.php", handleSync);
        group.MapPost("orgamax_osc_sync.php", handleSync);
        
        group.MapGet("orgamax_osc_del.php", async (IServiceProvider prov, [FromQuery] long id) =>
        {
            var service = ActivatorUtilities.CreateInstance<T>(prov);

            try {
                await service.StatusChange(id);
                return OrgamaxResults.Success("Success!");
            } catch (Exception e) {
                return OrgamaxResults.Error(e);
            }
        });
        
        return app;
    }
}