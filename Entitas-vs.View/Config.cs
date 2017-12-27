using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Entitas_vs.View.ViewModels;

namespace Entitas_vs.View
{
    static class Config
    {
        public static event Action Saved;

        public static ConfigData Load(string folderPath)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(ConfigData));
            string fileToLoad = Path.Combine(folderPath, "entitas-vs.xml");
            if(!File.Exists(fileToLoad))
                return new ConfigData();

            ConfigData data;
            using (FileStream fileStream = new FileStream(fileToLoad, FileMode.Open))
            {
                data = (ConfigData)serializer.ReadObject(fileStream);
            }
            data.Generators = data.Generators ?? new ObservableCollection<GeneratorData>();
            data.TriggerGroups = data.TriggerGroups ?? new ObservableCollection<TriggerGroup>();
            return data;
        }

        public static void Save(ConfigData data, string folderPath)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(ConfigData));
            using (FileStream fileStream = new FileStream(Path.Combine(folderPath, "entitas-vs.xml"), FileMode.OpenOrCreate))
            {
                var writerSettings = new XmlWriterSettings { Indent = true };
                using (XmlWriter writer = XmlWriter.Create(fileStream, writerSettings))
                {
                    serializer.WriteObject(writer, data);
                }
            }
            Saved?.Invoke();
        }
    }
}
