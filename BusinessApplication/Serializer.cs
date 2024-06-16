using BusinessApplication.Model;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

public static class Serializer
{
    public static string ToJson<T>(List<T> items, bool indented = true)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = indented
        };
        return JsonSerializer.Serialize(items, options);
    }

    public static List<T> FromJson<T>(string json)
    {
        return JsonSerializer.Deserialize<List<T>>(json) ?? [];
    }

    public static string ToXml<T>(List<T> items)
    {
        var serializer = new XmlSerializer(typeof(List<T>));

        using (StringWriter writer = new StringWriter())
        {
            serializer.Serialize(writer, items);
            return writer.ToString();
        }
    }

    public static List<T> FromXml<T>(string xml)
    {
        var serializer = new XmlSerializer(typeof(List<T>));
        using (var reader = new StringReader(xml))
        {
            return serializer.Deserialize(reader) as List<T> ?? [];
        }
    }
}
