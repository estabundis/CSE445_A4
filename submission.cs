using System;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Program
    {
        // These three must match exactly (case‑sensitive) for the autograder to pick them up:
        public static string xmlURL      = "https://estabundis.github.io/CSE445_A4/Hotels.xml";
        public static string xmlErrorURL = "https://estabundis.github.io/CSE445_A4/HotelsErrors.xml";
        public static string xsdURL      = "https://estabundis.github.io/CSE445_A4/Hotels.xsd";

        public static void Main(string[] args)
        {
            // Q3: call Verification on clean and error XML, then Xml2Json
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1 – validate XML against XSD, returning “No Error” or the concatenated
        // schema‐validation messages
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                var schemas = new XmlSchemaSet();
                schemas.Add(null, xsdUrl);

                var settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema,
                    Schemas = schemas
                };

                string errors = "";
                settings.ValidationEventHandler += (sender, e) =>
                {
                    errors += e.Message + Environment.NewLine;
                };

                using (var reader = XmlReader.Create(xmlUrl, settings))
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

        // Q2.2 – load XML and convert it to an indented JSON string
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(xmlUrl);

                // This must deserialize cleanly with JsonConvert.DeserializeXmlNode(jsonText)
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