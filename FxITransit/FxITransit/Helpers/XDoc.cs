using PCLStorage;
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
            return xDocument.Descendants().Where(p => p.Name.LocalName == nodeName).ToList();
        }

        public static IEnumerable<XElement> GetDescendantElements(this XElement xDocument, string nodeName)
        {
            return xDocument.Descendants().Where(p => p.Name.LocalName == nodeName);
        }

        public static IEnumerable<XElement> GetChildElements(this XElement xElement, string nodeName)
        {
           return  xElement.Elements().Where(p => p.Name.LocalName == nodeName);
        }
        public static bool HasValue(this string item)
        {
           return !string.IsNullOrWhiteSpace(item);
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

        public static T TryGetAttribute<T>(this XElement element, string attName)
        {
            T val = default(T);
            var stringVal = element.GetAttribute(attName);

            try
            {
                val =(T) Convert.ChangeType(stringVal, typeof(T));
            }
            catch
            {
                val = default(T);
            }
            return val;
        }

        public static XDocument LoadXml(string xml)
        {
            TextReader tr = new StringReader(xml);
            XDocument doc = XDocument.Load(tr);
            return doc;
        }
    }
}
