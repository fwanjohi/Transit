using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FxITransit.Helpers
{
    public static class XDoc
    {
        public static IEnumerable<XElement> GetDescendantElements(this XDocument xDocument, string nodeName)
        {
            return xDocument.Descendants().Where(p => p.Name.LocalName == nodeName);
        }

        public static string GetAttribute(this XElement xElement, string attName)
        {
            try
            {
                return xElement.Attribute(attName).Value;
            }
            catch
            {
                return string.Empty;
            }
            
        }

        public static XDocument LoadXml(string xml)
        {
            TextReader tr = new StringReader(xml);
            XDocument doc = XDocument.Load(tr);
            return doc;
        }
    }
}
