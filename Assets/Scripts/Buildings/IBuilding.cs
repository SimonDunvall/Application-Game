using System;
using Assets.Scripts.Map;

namespace Assets.Scripts.Buildings
{
    internal interface IBuilding
    {
        void PlaceBuilding(CustomCursor customCursor, Tile[] tiles);
    }
}