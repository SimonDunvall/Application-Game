using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public interface IBuilding
    {
        string Type { get; set; }
        Vector3 Pos { get; set; }

        Sprite GetSprite();
        void PlaceBuilding();
    }
}