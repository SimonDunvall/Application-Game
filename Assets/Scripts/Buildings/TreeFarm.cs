using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Map;
using Assets.Scripts.SaveSystem;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public class TreeFarm : MonoBehaviour, IResourceBuilding
    {
        public string Type => "TreeFarm";
        private int _level = 1;
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        public int ResourcePerMinute;
        public int InnerStorageSize;
        public int InnerStorage { get; set; }
        public float TimeLeft { get; set; }
        private float nextIncreaseTime = 60f;
        public string ResourceType => "Wood";

        private void Awake()
        {
            SaveSystemManager.treeFarms.Add(this);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            UpdateTimer();
            UpdateResource();
        }

        private void UpdateTimer()
        {
            TimeLeft = nextIncreaseTime - Time.time;
            UiManager.instance.UpdateTimerText((int)TimeLeft, GetInstanceID());
        }

        private void UpdateResource()
        {
            if (TimeLeft <= 0 && InnerStorage < InnerStorageSize)
            {
                nextIncreaseTime = Time.time + 60f;
                InnerStorage += ResourcePerMinute;
                UiManager.instance.UpdateResourceText(InnerStorage.ToString(), ResourceType, GetInstanceID());
            }
            if (InnerStorage > InnerStorageSize)
                InnerStorage = InnerStorageSize;
        }

        public void CollectStorage()
        {
            if (InnerStorage > 0)
            {
                SaveSystemManager.resources.wood += InnerStorage;
                InnerStorage = 0;
                UiManager.instance.UpdateResourceText(InnerStorage.ToString(), ResourceType, GetInstanceID());
            }
        }

        public Sprite GetSprite()
        {
            return GetComponent<SpriteRenderer>().sprite;
        }

        public void PlaceBuilding()
        {
            Tile nearestTile = Tile.GetNearestTile();

            List<Tile> neighbouringTiles = nearestTile.FindAllTileNeighbors();
            neighbouringTiles.Add(nearestTile);
            if (neighbouringTiles.Count == 9 && neighbouringTiles.All(tile => !tile.isOccupied))
            {
                CreateObject(this, nearestTile.transform.position);

                nearestTile.SetCloseTilesOccupied();
                StaticClass.GetCustomCursor().DisableCursor();
                GameManager.instance.ResetValues();
            }
        }

        public static TreeFarm CreateObject(TreeFarm preFab, Vector3 position)
        {
            return Instantiate(preFab, position, Quaternion.identity);
        }

        public void LevelUp()
        {
            Level += 1;
            UiManager.instance.UpdateLevelText(Level, GetInstanceID());
        }
    }
}