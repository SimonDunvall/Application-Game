using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Buildings;
using Assets.Scripts.Map;
using System.Linq;
using Assets.Scripts.SaveSystem.Data;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.SaveSystem
{
    public class SaveSystemManager : MonoBehaviour
    {
        [SerializeField] public Building buildingPrefab;

        public static List<Building> buildings = new List<Building>();
        public static List<Tile> tiles = new List<Tile>();
        static bool hasLoadedSave = false;

        private FileDataHandler dataHandler;

        public static SaveSystemManager instance { get; private set; }

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

            this.dataHandler = new FileDataHandler(Application.persistentDataPath);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start()
        {
            //Loads tiles from before the scene switched
            if (StaticClass.tilesToSave.Count() > 0)
            {
                foreach (var pos in StaticClass.tilesToSave)
                {
                    var tile = Tile.FindTile(pos);
                    tile.isOccupied = true;
                    tiles.Add(tile);
                }
                StaticClass.tilesToSave.Clear();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!hasLoadedSave)
            {
                hasLoadedSave = true;
                LoadGame();
            }
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        public void LoadGame()
        {
            dataHandler.Load(new BuildingData());
            dataHandler.Load(new TileData());
        }


        public void SaveGame()
        {
            dataHandler.Save((new BuildingData()), buildings.Count());
            dataHandler.Save((new TileData()), tiles.Count());
        }

        void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}