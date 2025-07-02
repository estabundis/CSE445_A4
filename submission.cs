using System;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Program
    {
        // urls for each of my instances to submit
        public static string xmlURL      = "https://estabundis.github.io/CSE445_A4/Hotels.xml";
        public static string xmlErrorURL = "https://estabundis.github.io/CSE445_A4/HotelsErrors.xml";
        public static string xsdURL      = "https://estabundis.github.io/CSE445_A4/Hotels.xsd";

        public static void Main(string[] args)
        {
            // VALIDATE xml
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            // validate error xml
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            // CONVERTS xml to JSON
            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // this function will validate XML against the XSD at url
        // basically returns schema errors OR just no error
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                // instantiate/load schema
                var schemas = new XmlSchemaSet();
                schemas.Add(null, xsdUrl);

                // reader for XSL validation
                var settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema,
                    Schemas = schemas
                };


                string errors = "";
                settings.ValidationEventHandler += (sender, e) =>
                {
                    // COLLECT ALL MSGS
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

        // load XML and convert to an indented JSON string
        // JSON must deserialize via JSON function
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(xmlUrl);

                // this MUST deserialize cleanly with JsonConvert.DeserializeXmlNode()
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