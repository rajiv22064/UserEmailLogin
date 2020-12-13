using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace UITestAutomation
{
   public class UIHelpers
    {
        public static string GetXpathValue(string objectFile, string pageName, string controlName)
        {
            Logger.LogInfo(string.Format("Looking for xpath in file: '{0}.xml', under page: '{1}', for object: '{2}'", objectFile, pageName, controlName));
            string filename = ".\\ObjectRepository\\" + objectFile + ".xml";
            XElement xElement = XElement.Load(filename);
            XElement dataXml = new XElement("ObjectRepositorySuite");
            dataXml.Add(xElement.Elements());
            string attributeValue = string.Empty;

            IEnumerable<XElement> xElements = from tcXml in dataXml.Elements()
                                              where tcXml.Attributes().Any(attr => Equals(attr.Name.LocalName, "PageName"))
                                              && Equals(tcXml.Attribute("PageName").Value, pageName)
                                              select tcXml;

            XElement testCaseXml = xElements.FirstOrDefault();

            IEnumerable<XElement> singleControl = from xPage in testCaseXml.Elements()
                                                  where xPage.Attributes().Any(attr => Equals(attr.Value, controlName))
                                                  select xPage;
            XElement controlToFind = singleControl.FirstOrDefault();

            IEnumerable<string> values = from xa in controlToFind.Attributes()
                                         where xa.Name == "Value"
                                         select xa.Value;

            attributeValue = values.Any() ? values.ToList()[0] : string.Empty;
            Logger.Log("Xpath of '"+ controlName + "':' " + attributeValue+"'");
            return attributeValue;
        }
    }
}
