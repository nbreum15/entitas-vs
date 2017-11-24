namespace EntitasVSGenerator
{
    using EntitasVSGenerator.Logic;
    public class ConfigureTabModel
    {
        public ProjectItem[] ProjectItems { get; set; }

        public ConfigureTabModel(ProjectItem[] projectItems)
        {
            ProjectItems = projectItems;
        }
    }
}