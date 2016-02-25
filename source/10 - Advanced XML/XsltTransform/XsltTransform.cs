using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Xsl;

namespace XsltTransform
{
    [TestClass]
    public class XsltTransform
    {
        [TestMethod]
        public void TestMethod1()
        {
            var xsl = new XslCompiledTransform(true);
            var settings = new XsltSettings { EnableScript = true };
            xsl.Load("books.xslt", settings, null);
            xsl.Transform("books.xml", "report.html");
        }
    }
}
