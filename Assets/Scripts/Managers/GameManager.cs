using UnityEngine;
using Assets.Scripts.Map;
using Assets.Scripts.Buildings;
using Assets.Scripts.SaveSystem;
using System.Linq;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        private IBuilding buildingToPlace;
        public CustomCursor customCursor;
        public Tile[] tiles;

        public static GameManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            buildingToPlace = StaticClass.BoughtBuilding;
            customCursor = StaticClass.CustomCursor;
        }

        private void Update()
        {
            if (buildingToPlace != null && Input.GetMouseButton(0))
                buildingToPlace.PlaceBuilding();
        }

        internal void ResetValues()
        {
            buildingToPlace = null;
            customCursor = null;

            StaticClass.BoughtBuilding = null;
            StaticClass.CustomCursor = null;
        }
    }
}