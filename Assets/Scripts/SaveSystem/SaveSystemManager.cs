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
        [SerializeField] public TestBuilding buildingPrefab;
        [SerializeField] public TreeFarm treeFarmPrefab;
        [SerializeField] public Mine minePrefab;

        public static List<TestBuilding> testBuildings = new List<TestBuilding>();
        public static List<TreeFarm> treeFarms = new List<TreeFarm>();
        public static List<Mine> mine = new List<Mine>();
        public static List<Tile> tiles = new List<Tile>();
        public static Resources resources = new Resources()
        {
            gold = 0,
            wood = 0,
            stone = 0,
            metal = 0
        };
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
            if (StaticClass.GettilesToSave().Count() > 0)
            {
                foreach (var pos in StaticClass.GettilesToSave())
                {
                    var tile = Tile.FindTile(pos);
                    tile.isOccupied = true;
                    tiles.Add(tile);
                }
                StaticClass.GettilesToSave().Clear();
            }
        }

        private void Update()
        {
            Resources.UpdateResources();
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
            dataHandler.Load(new TestBuildingData());
            dataHandler.Load(new TreeFarmData());
            dataHandler.Load(new MineData());
            dataHandler.Load(new TileData());
            dataHandler.Load(new ResourcesData());
        }


        public void SaveGame()
        {
            dataHandler.Save((new TestBuildingData()), testBuildings.Count());
            dataHandler.Save((new TreeFarmData()), treeFarms.Count());
            dataHandler.Save((new MineData()), mine.Count());
            dataHandler.Save((new TileData()), tiles.Count());
            dataHandler.Save((new ResourcesData()), 1);
        }

        void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}