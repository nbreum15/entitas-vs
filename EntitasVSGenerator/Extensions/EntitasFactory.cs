using Entitas.CodeGeneration.CodeGenerator;
using Entitas.Utils;

namespace EntitasVSGenerator.Extensions
{
    static class EntitasFactory
    {
        public static Preferences GetPreferences(string solutionDirectory)
        {
            string propertiesPath = solutionDirectory + "\\" + Preferences.DEFAULT_PROPERTIES_PATH;
            string userPropertiesPath = solutionDirectory + "\\" + Preferences.DEFAULT_USER_PROPERTIES_PATH;
            return new Preferences(propertiesPath, userPropertiesPath);
        }

        public static CodeGenerator GetCodeGenerator(string solutionDirectory)
        {
            return CodeGeneratorUtil.CodeGeneratorFromPreferences(GetPreferences(solutionDirectory));
        }
    }
}
