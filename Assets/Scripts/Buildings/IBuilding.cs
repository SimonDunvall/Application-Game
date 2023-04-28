using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public interface IBuilding
    {
        int InstaceId { get; }
        string Type { get; }
        int Level { get; set; }

        Sprite GetSprite();
        void PlaceBuilding();
        void LevelUp();
        int GetInstanceID();
        int GetMaxLevel();
        bool IsMaxLevel();
        Dictionary<string, int> GetUpgradeCost();
    }
}