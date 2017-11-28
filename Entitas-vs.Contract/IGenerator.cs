namespace Entitas_vs.Contract
{
    public interface IGenerator
    {
        string[] Generate();
        string TargetDirectory { get; }
    }
}
