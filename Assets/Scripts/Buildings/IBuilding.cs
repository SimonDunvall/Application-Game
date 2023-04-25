using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public interface IBuilding
    {
        string Type { get; }

        Sprite GetSprite();
        void PlaceBuilding();
    }
}