using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    internal interface IBuilding
    {
        string Type { get; set; }
        Vector3 Pos { get; set; }

        void PlaceBuilding(CustomCursor customCursor);
    }
}