using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Entitas_vs.View.ViewModels
{
    [DataContract]
    class ConfigData
    {
        [DataMember] public ObservableCollection<TriggerGroup> TriggerGroups { get; set; }
        [DataMember] public ObservableCollection<GeneratorData> Generators { get; set; }

        public string GetGeneratorPath(string generatorName)
        {
            return Generators?.FirstOrDefault(data => data.Name == generatorName)?.GeneratorPath;
        }
    }
}