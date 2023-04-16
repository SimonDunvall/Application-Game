using System.Collections.Generic;
using Assets.Scripts.Buildings;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts
{
    public static class StaticClass
    {
        public static Building BoughtBuilding { get; set; }
        public static CustomCursor CustomCursor { get; set; }
        public static List<Vector3> tilesToSave { get; set; } = new List<Vector3>();
    }
}