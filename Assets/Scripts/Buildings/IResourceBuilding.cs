using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assets.Scripts.Buildings
{
    public interface IResourceBuilding
    {
        int InnerStorage { get; set; }
        float TimeLeft { get; set; }
        string ResourceType { get; }

        void CollectStorage();
        int GetInstanceID();
    }
}