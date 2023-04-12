using UnityEngine;
using Assets.Scripts.Map;
using Assets.Scripts.Buildings;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        private IBuilding buildingToPlace;
        private CustomCursor customCursor;
        public Tile[] tiles;

        public static GameManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
                Debug.LogError("Found more than one Game Managers in the scene");

            instance = this;
        }

        private void Start()
        {
            buildingToPlace = StaticClass.BoughtBuilding;
            customCursor = StaticClass.CustomCursor;
        }

        private void Update()
        {
            if (buildingToPlace != null && Input.GetMouseButton(0))
                buildingToPlace.PlaceBuilding(customCursor);
        }
    }
}