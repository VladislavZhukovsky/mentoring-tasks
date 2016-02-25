using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Xml.Schema;

namespace XsdVerify
{
    [TestClass]
    public class VerifyXml
    {
        XmlReaderSettings settings;

        public void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        [TestInitialize]
        public void Init()
        {
            settings = new XmlReaderSettings();
            settings.Schemas.Add("http://library.by/catalog", "books.xsd");
            settings.ValidationType = ValidationType.Schema;
        }

        [TestMethod]
        public void CheckValidXML()
        {
            try
            {
                XmlReader reader = XmlReader.Create("books.xml", settings);
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                document.Validate(eventHandler);
                reader.Close();
            }
            catch (XmlSchemaValidationException ex)
            {
                Console.WriteLine(String.Format("{0} \nLine: {1}, position: {2}", ex.Message, ex.LineNumber, ex.LinePosition));
            }
        }

        [TestMethod]
        public void CheckInvalidXML1()
        {
            try
            {
                XmlReader reader = XmlReader.Create("books_invalid1.xml", settings);
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                document.Validate(eventHandler);
                reader.Close();
            }
            catch (XmlSchemaValidationException ex)
            {
                Console.WriteLine(String.Format("{0} \nLine: {1}, position: {2}", ex.Message, ex.LineNumber, ex.LinePosition));
            }
        }

        [TestMethod]
        public void CheckInvalidXML2()
        {
            try
            {
                XmlReader reader = XmlReader.Create("books_invalid2.xml", settings);
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                document.Validate(eventHandler);
                reader.Close();
            }
            catch (XmlSchemaValidationException ex)
            {
                Console.WriteLine(String.Format("{0} \nLine: {1}, position: {2}", ex.Message, ex.LineNumber, ex.LinePosition));
            }
        }

    }
}
