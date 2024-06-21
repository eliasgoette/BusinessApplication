using BusinessApplication.Model;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

public static class CustomerSerializer
{
    public static string ToJson(List<Customer> items, bool indented = true)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = indented
        };
        return JsonSerializer.Serialize(items, options);
    }

    public static List<Customer> FromJson(string json)
    {
        return JsonSerializer.Deserialize<List<Customer>>(json) ?? [];
    }

    public static string ToXml(List<Customer> items)
    {
        var serializer = new XmlSerializer(typeof(Customers));

        using (var writer = new StringWriter())
        {
            serializer.Serialize(writer, new Customers { Items = items });
            return writer.ToString();
        }
    }

    public static List<Customer> FromXml(string xml)
    {
        var serializer = new XmlSerializer(typeof(Customers));
        using (var reader = new StringReader(xml))
        {
            var customersObj = serializer.Deserialize(reader) as Customers;
            return customersObj?.Items ?? [];
        }
    }

    [XmlRoot("Customers")]
    public class Customers
    {
        [XmlElement("Customer")]
        public List<Customer> Items { get; set; }

        public Customers()
        {
            Items = [];
        }
    }
}