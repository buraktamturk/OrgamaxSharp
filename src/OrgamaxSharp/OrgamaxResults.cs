using System.Xml;
using Microsoft.AspNetCore.Http;
using OrgamaxSharp.Extensions;

namespace OrgamaxSharp;

public static class OrgamaxResults
{
    public static IResult Error(string message, string? function = null)
    {
        return Results.Stream(async stream =>
        {
            await using var xmlWriter = XmlWriter.Create(stream, EndpointExtensions._settings);

            await xmlWriter.WriteStartDocumentAsync();
            await xmlWriter.WriteStartElementAsync(null, "orgamax_phperror", null);

            await xmlWriter.WriteElementStringAsync(null, "message", null, message);
            await xmlWriter.WriteElementStringAsync(null, "function", null, function ?? "");

            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.WriteEndDocumentAsync();
        });
    }
    
    public static IResult Error(Exception e)
    {
        return Error(e.Message);
    }

    public static IResult Success(string message, string? status = null)
    {
        return Results.Stream(async stream =>
        {
            await using var xmlWriter = XmlWriter.Create(stream, EndpointExtensions._settings);

            await xmlWriter.WriteStartDocumentAsync();
            await xmlWriter.WriteStartElementAsync(null, "orgamax_phpsuccess", null);

            await xmlWriter.WriteElementStringAsync(null, "message", null, message);
            await xmlWriter.WriteElementStringAsync(null, "status", null, status ?? "");

            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.WriteEndDocumentAsync();
        });
    }
    
    public static IResult Imported(int allCount, int importedCount)
    {
        return Results.Stream(async stream =>
        {
            await using var xmlWriter = XmlWriter.Create(stream, EndpointExtensions._settings);

            await xmlWriter.WriteStartDocumentAsync();
            await xmlWriter.WriteStartElementAsync(null, "Exportprotokoll", null);

            await xmlWriter.WriteElementStringAsync(null, "Export_Status", null, importedCount > 0 ? "SUCCESS" : "ROLLBACK");
            await xmlWriter.Wr("Anzahl_Datensaetze_Gesamt", allCount);
            await xmlWriter.Wr("Anzahl_Datensaetze_Erfolgreich_Uebergeben", importedCount);

            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.WriteEndDocumentAsync();
        });
    }
    
    public static IResult PagingInformation(int articleCount)
    {
        return Results.Stream(async stream =>
        {
            await using var xmlWriter = XmlWriter.Create(stream, EndpointExtensions._settings);

            await xmlWriter.WriteStartDocumentAsync(true);
            await xmlWriter.WriteStartElementAsync(null, "PagingInformations", null);

            await xmlWriter.Wr("ArticleCount", articleCount);

            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.WriteEndDocumentAsync();
        });
    }
}