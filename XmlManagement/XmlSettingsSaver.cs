using DifficultyProcessor.Settings;
using System.Xml.Linq;

namespace DifficultyProcessor.XmlManagement
{
    internal class XmlSettingsSaver
    {
        public void SaveSettings()
        {
            var initialXml = new XDocument(
             new XElement("Data",
                 new XElement("StartDirectory"),
                 new XElement("TargetFolder"),
                 new XElement("ApiKey"),
                 new XElement("DesiredDifficulty"),
                 new XElement("DesiredMod")
             )
         );

            foreach (var element in initialXml.Root.Elements())
            {
                Console.Write($"Enter {element.Name}: ");

                var inputValue = Console.ReadLine();
                element.Value = inputValue;
            }

            initialXml.Save(OsuSettings.SettingsPath);
        }
    }
}
