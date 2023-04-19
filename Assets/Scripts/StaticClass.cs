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

        private static List<Vector3> tilesToSave = new List<Vector3>();

        public static List<Vector3> GettilesToSave()
        {
            return tilesToSave;
        }

        public static void SettilesToSave(List<Vector3> value)
        {
            tilesToSave = value;
        }
    }
}