using System.Globalization;
using System.Xml;

namespace OrgamaxSharp.Extensions;

public static class XmlWriterExtensions
{
    public static async Task Wr(this XmlWriter writer, string name, string value)
    {
        await writer.WriteStartElementAsync(null, name, null);
        if (!string.IsNullOrWhiteSpace(value)) {
            await writer.WriteCDataAsync(value);
        }
        await writer.WriteEndElementAsync();
    }
    
    public static async Task Wr(this XmlWriter writer, string name, int? value)
    {
        await writer.WriteElementStringAsync(null, name, null, value?.ToString(CultureInfo.InvariantCulture));
    }
    
    public static async Task Wr(this XmlWriter writer, string name, long? value)
    {
        await writer.WriteElementStringAsync(null, name, null, value?.ToString(CultureInfo.InvariantCulture));
    }
    
    public static async Task Wr(this XmlWriter writer, string name, decimal? value)
    {
        await writer.WriteElementStringAsync(null, name, null, value?.ToString(CultureInfo.InvariantCulture));
    }
    
    public static async Task Wr(this XmlWriter writer, string name, DateTimeOffset? value)
    {
        await writer.WriteStartElementAsync(null, name, null);
        if (value is not null) {
            await writer.WriteCDataAsync(value.Value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        await writer.WriteEndElementAsync();
    }
}