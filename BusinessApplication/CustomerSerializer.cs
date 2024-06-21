using BusinessApplication.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        return JsonSerializer.Deserialize<List<Customer>>(json) ?? new List<Customer>();
    }

    public static string ToXml(List<Customer> items)
    {
        var serializer = new XmlSerializer(typeof(CustomersXmlDto));

        using (var writer = new StringWriter())
        {
            var dtoList = items.ConvertAll(x => new CustomerXmlDto(x));
            serializer.Serialize(writer, new CustomersXmlDto { Items = dtoList });
            return writer.ToString();
        }
    }

    public static List<Customer> FromXml(string xml)
    {
        var serializer = new XmlSerializer(typeof(CustomersXmlDto));
        using (var reader = new StringReader(xml))
        {
            var customersObj = serializer.Deserialize(reader) as CustomersXmlDto;
            return customersObj?.Items.Select(x => x.ToCustomer()).ToList() ?? new List<Customer>();
        }
    }

    [XmlRoot("Customers")]
    public class CustomersXmlDto
    {
        [XmlElement("Customer")]
        public List<CustomerXmlDto> Items { get; set; } = new List<CustomerXmlDto>();
    }

    public class CustomerXmlDto
    {
        public CustomerXmlDto() { }

        public CustomerXmlDto(Customer customer)
        {
            Id = customer.Id;
            CustomerAddress = customer.CustomerAddress;
            CustomerNumber = customer.CustomerNumber;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Email = customer.Email;
            Website = customer.Website;
            PasswordHash = customer.PasswordHash;
        }

        [XmlIgnore]
        public int Id { get; set; }

        public Address CustomerAddress { get; set; }

        [XmlAttribute(nameof(CustomerNumber))]
        public string CustomerNumber { get; set; }

        [XmlIgnore]
        public string FirstName { get; set; }

        [XmlIgnore]
        public string LastName { get; set; }

        [XmlElement("Name")]
        public string Name
        {
            get => $"{FirstName} {LastName}";
            set
            {
                var parts = value.Split(' ');
                if (parts.Length > 1)
                {
                    FirstName = parts[0];
                    LastName = string.Join(" ", parts.Skip(1));
                }
            }
        }

        public string Email { get; set; }
        public string Website { get; set; }
        public string PasswordHash { get; set; }

        public Customer ToCustomer()
        {
            return new Customer
            {
                Id = Id,
                CustomerAddress = CustomerAddress,
                CustomerNumber = CustomerNumber,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Website = Website,
                PasswordHash = PasswordHash
            };
        }
    }
}