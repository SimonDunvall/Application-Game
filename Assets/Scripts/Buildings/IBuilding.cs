using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public interface IBuilding
    {
        string Type { get; }
        int Level { get; set; }

        Sprite GetSprite();
        void PlaceBuilding();
        void LevelUp();
        int GetInstanceID();
    }
}