using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Linq;
using System.Xml.XPath;

namespace ConsoleTranslator
{
    public class Resources
    {
        static public IEnumerable<string> GetFileList()
        {
        return Directory.GetFiles(@"C:\Users\krks\Desktop\Resources", "*resx");
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"C:\Users\krks\Desktop\Resources\Activity.resx");
            List<string> Languages = new List<string>();
            Languages.Add("EN ");
            Languages.Add("FR ");
            Languages.Add("PL ");
            Languages.Add("DE ");
            Languages.Add("GR ");
            Languages.Add("ES ");
            Languages.Add("NE ");
            Languages.Add("NO ");
            Languages.Add("CZ ");
            Languages.Add("IT");

            var document = new XDocument();
            var root = new XElement("root");
            var languages = new XElement("Language", Languages);
            
            var resources = new XElement("Resources");
            //var translation = new XElement("Translation");
            root.Add(resources);
            resources.Add(languages);
            // resources.Add(translation);
            //resources.FirstNode.AddAfterSelf(translation);

            foreach (string resourceFile in Resources.GetFileList())
            { 
                var file = new XElement("File", Path.GetFileNameWithoutExtension(resourceFile));
                var path = new XElement("Path", resourceFile);
                resources.Add(file);
                resources.Add(path);
               
                var keys = new XElement("Keys");
                resources.Add(keys);
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.GetFullPath(resourceFile));
                XmlNodeList elemList = doc.GetElementsByTagName("data");
                
                
                for (int i = 0; i < elemList.Count; i++)
                {
                    string datanames = elemList[i].Attributes["name"].Value;
                    string datavalues = elemList[i].InnerText;
                    string all ="DM." + Path.GetFileNameWithoutExtension(resourceFile) + "." + datanames;
                    var translationKey = new XElement("Translation");
                    var translationKeyAttribute = new XAttribute("Key" , all);
                    var value = new XElement("value", datavalues);
                    var englishLanguage = new XAttribute("Lang", Languages[0]);
                    keys.Add(translationKey);
                    translationKey.Add(value);
                    translationKey.Add(translationKeyAttribute);
                    value.Add(englishLanguage);
                    for (int j = 1; j < Languages.Count; j++)
                    {
                        var translateValue = new XElement("value");
                        var lang = new XAttribute("Lang", Languages[j]);
                        translationKey.Add(translateValue);
                        translateValue.Add(lang);
                    }
                }
            }
            
            document.Add(root);
           
            document.Save(@"C:\Users\krks\Desktop\ResourcesResult\ConsoleTranslatorResult.xml");
        }
    }
}







