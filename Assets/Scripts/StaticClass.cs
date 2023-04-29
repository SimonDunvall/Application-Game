using System.Collections.Generic;
using Assets.Scripts.Buildings;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts
{
    public static class StaticClass
    {
        private static IBuilding boughtBuilding;

        public static IBuilding GetBoughtBuilding()
        {
            return boughtBuilding;
        }

        public static void SetBoughtBuilding(IBuilding value)
        {
            boughtBuilding = value;
        }

        private static CustomCursor customCursor;

        public static CustomCursor GetCustomCursor()
        {
            return customCursor;
        }

        public static void SetCustomCursor(CustomCursor value)
        {
            customCursor = value;
        }

        public static List<Vector3> TilesToSave { get; set; } = new List<Vector3>();

        public static int HomeLevel { get; set; }
    }
}