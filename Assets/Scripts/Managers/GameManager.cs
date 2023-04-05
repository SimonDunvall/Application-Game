using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Assets.Scripts.Map;
using Assets.Scripts.Buildings;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        private IBuilding buildingToPlace;
        private CustomCursor customCursor;
        public Tile[] tiles;

        private void Start()
        {
            buildingToPlace = StaticClass.BoughtBuilding;
            customCursor = StaticClass.CustomCursor;
        }

        private void Update()
        {
            if (buildingToPlace != null)
                buildingToPlace.PlaceBuilding(customCursor, tiles);
        }
    }
}