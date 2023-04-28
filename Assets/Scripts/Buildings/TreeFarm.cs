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
        public int InstaceId => GetInstanceID();
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
        public int MaxLevel;
        public int ResourcePerMinute;
        public int InnerStorageSize;
        public List<string> InnerStorage { get; set; } = new List<string>();
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
            if (InnerStorage.Count() < InnerStorageSize)
            {
                TimeLeft = nextIncreaseTime - Time.time;
                UiManager.instance.UpdateTimerText(InstaceId, false, (int)TimeLeft);
            }
            else
            {
                UiManager.instance.UpdateTimerText(InstaceId, true);
            }
        }

        private void UpdateResource()
        {
            if (TimeLeft <= 0 && InnerStorage.Count() < InnerStorageSize)
            {
                nextIncreaseTime = Time.time + 60f;
                InnerStorage.AddRange(Enumerable.Repeat(ResourceType, ResourcePerMinute));
            }
            if (InnerStorage.Count() > InnerStorageSize)
            {
                int itemsToRemove = InnerStorage.Count() - InnerStorageSize;
                int startIndex = InnerStorage.Count() - itemsToRemove;
                InnerStorage.RemoveRange(startIndex, itemsToRemove);
            }
            UiManager.instance.UpdateResourceText(InnerStorage.Count().ToString(), ResourceType, InstaceId);
        }

        public void CollectStorage()
        {
            if (InnerStorage.Count() > 0)
            {
                SaveSystemManager.resources.wood += InnerStorage.Count();
                if (InnerStorage.Count() >= InnerStorageSize)
                {
                    nextIncreaseTime = Time.time + 10f;
                }
                InnerStorage.Clear();
                UiManager.instance.UpdateResourceText(InnerStorage.Count().ToString(), ResourceType, InstaceId);
                Resources.UpdateResources();
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
            if (Level < MaxLevel)
            {
                Level += 1;
                UiManager.instance.UpdateLevelText(this);
            }
        }

        public int GetMaxLevel()
        {
            return MaxLevel;
        }

        public bool IsMaxLevel()
        {
            if (MaxLevel <= Level)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}