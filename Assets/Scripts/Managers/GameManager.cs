using UnityEngine;
using Assets.Scripts.Map;
using Assets.Scripts.Buildings;
using TMPro;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public TMP_Text goldDisplay;
        public TMP_Text woodDisplay;
        public TMP_Text stoneDisplay;
        public TMP_Text MetalDisplay;

        private IBuilding buildingToPlace;
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
            buildingToPlace = StaticClass.GetBoughtBuilding();
        }

        private void Update()
        {
            if (buildingToPlace != null && Input.GetMouseButton(0))
                buildingToPlace.PlaceBuilding();
        }

        internal void ResetValues()
        {
            buildingToPlace = null;

            StaticClass.SetBoughtBuilding(null);
            StaticClass.SetCustomCursor(null);
        }
    }
}