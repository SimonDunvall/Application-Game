using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Buildings;
using Assets.Scripts.Map;
using System.Linq;
using Assets.Scripts.SaveSystem.Data;

namespace Assets.Scripts.SaveSystem
{
    public class SaveSystemManager : MonoBehaviour
    {
        [SerializeField] public Building buildingPrefab;

        public static List<Building> buildings = new List<Building>();
        public static List<Tile> tiles = new List<Tile>();

        private FileDataHandler dataHandler;

        public static SaveSystemManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
                Debug.LogError("Found more than one Save Managers in the scene");

            instance = this;
        }

        private void Start()
        {
            this.dataHandler = new FileDataHandler(Application.persistentDataPath);
            LoadGame();
        }


        private void OnApplicationQuit()
        {
            SaveGame();
        }

        void LoadGame()
        {
            dataHandler.Load(new BuildingData());
            dataHandler.Load(new TileData());
        }


        void SaveGame()
        {
            dataHandler.Save((new BuildingData()), SaveSystemManager.buildings.Count());
            dataHandler.Save((new TileData()), SaveSystemManager.tiles.Count());
        }
    }
}