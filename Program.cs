using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string xmlUrl = @"Hotels.xml";
            string xsdUrl = @"Hotels.xsd";
            string errorXmlUrl = @"HotelErrors.xml";

            Console.WriteLine("Validating Hotels.xml:");
            Console.WriteLine(Verification(xmlUrl, xsdUrl));

            Console.WriteLine("\nValidating HotelsErrors.xml:");
            Console.WriteLine(Verification(errorXmlUrl, xsdUrl));

            Console.WriteLine("\nConverting Hotels.xml to JSON:");
            Console.WriteLine(Xml2Json(xmlUrl));
        }

        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(null, xsdUrl);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemas;

                string errors = string.Empty;
                settings.ValidationEventHandler += (sender, e) =>
                {
                    errors += e.Message + Environment.NewLine;
                };

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { }
                }

                return string.IsNullOrEmpty(errors) ? "No Error" : errors.Trim();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlUrl);

                string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
                return jsonText;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}