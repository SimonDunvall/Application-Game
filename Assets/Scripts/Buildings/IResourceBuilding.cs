using System.Collections.Generic;

namespace Assets.Scripts.Buildings
{
    public interface IResourceBuilding : IBuilding
    {
        List<string> InnerStorage { get; set; }
        float TimeLeft { get; set; }
        string ResourceType { get; }

        void CollectStorage();
    }
}