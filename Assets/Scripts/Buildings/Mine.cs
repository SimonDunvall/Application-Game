using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Map;
using Assets.Scripts.SaveSystem;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public class Mine : MonoBehaviour, IResourceBuilding
    {
        public int InstaceId => GetInstanceID();
        public string Type => "Mine";
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
        public int BaseGoldCost;
        public int BaseWoodCost;
        public int BaseStoneCost;
        public int BaseMetalCost;
        public int UpgradeGoldCost;
        public int UpgradeWoodCost;
        public int UpgradeStoneCost;
        public int UpgradeMetalCost;
        public int ResourcePerMinute;
        public int InnerStorageSize;
        public float LevelModifier;
        public int LevelToUnlockSecondResourceType;
        public List<string> InnerStorage { get; set; } = new List<string>();
        public float TimeLeft { get; set; }
        private float nextIncreaseTime = 10f;
        public string ResourceType => "Stone";
        public string SecondResourceType => "Metal";
        private string _choosenResourceType = "Stone";
        public string ChoosenResourceType { get { return _choosenResourceType; } set { _choosenResourceType = value; } }

        private void Awake()
        {
            SaveSystemManager.mine.Add(this);
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
                MapUiManager.instance.UpdateTimerText(InstaceId, false, (int)TimeLeft);
            }
            else
            {
                MapUiManager.instance.UpdateTimerText(InstaceId, true);
            }
        }

        private void UpdateResource()
        {
            if (TimeLeft <= 0 && InnerStorage.Count() < InnerStorageSize)
            {
                nextIncreaseTime = Time.time + 10f;
                InnerStorage.AddRange(Enumerable.Repeat(ChoosenResourceType, ResourcePerMinute).Concat(Enumerable.Repeat(ResourceType, (int)(Level * LevelModifier))));
            }
            if (InnerStorage.Count() > InnerStorageSize)
            {
                int itemsToRemove = InnerStorage.Count() - InnerStorageSize;
                int startIndex = InnerStorage.Count() - itemsToRemove;
                InnerStorage.RemoveRange(startIndex, itemsToRemove);
            }
            var resourceTypeText = ResourceType;
            if (UnlockedMetal())
            {
                resourceTypeText = $"{ResourceType} or {SecondResourceType}";
            }
            MapUiManager.instance.UpdateResourceText(InnerStorage.Count().ToString(), resourceTypeText, InstaceId);
        }

        public void CollectStorage()
        {
            if (InnerStorage.Count() > 0)
            {
                SaveSystemManager.resources.stone += InnerStorage.Where(r => r == ResourceType).Count();
                SaveSystemManager.resources.metal += InnerStorage.Where(r => r == SecondResourceType).Count();
                if (InnerStorage.Count() >= InnerStorageSize)
                {
                    nextIncreaseTime = Time.time + 10f;
                }
                InnerStorage.Clear();
                var resourceTypeText = ResourceType;
                if (UnlockedMetal())
                {
                    resourceTypeText = $"{ResourceType} or {SecondResourceType}";
                }
                MapUiManager.instance.UpdateResourceText(InnerStorage.Count().ToString(), resourceTypeText, InstaceId);
                Resources.UpdateResources();
            }
        }

        public Sprite GetSprite()
        {
            return GetComponent<SpriteRenderer>().sprite;
        }

        public void PlaceBuilding()
        {
            Tile nearestTile = Tile.GetNearestTile(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            List<Tile> neighbouringTiles = nearestTile.FindAllTileNeighbors();
            neighbouringTiles.Add(nearestTile);
            if (neighbouringTiles.Count == 9 && neighbouringTiles.All(tile => !tile.isOccupied))
            {
                Resources.Pay(GetCost());
                Resources.UpdateResources();

                CreateObject(this, nearestTile.transform.position);

                nearestTile.SetCloseTilesOccupied();
                StaticClass.GetCustomCursor().DisableCursor();
                GameManager.instance.ResetValues();
            }
        }

        public static Mine CreateObject(Mine preFab, Vector3 position)
        {
            return Instantiate(preFab, position, Quaternion.identity);
        }

        public void LevelUp()
        {
            if (Level < MaxLevel)
            {
                Resources.Pay(GetUpgradeCost());
                Resources.UpdateResources();
                Level += 1;
                var resourceTypeText = ResourceType;
                if (UnlockedMetal())
                {

                    MapUiManager.instance.UpdateResourceText(InnerStorage.Count().ToString(), resourceTypeText, InstaceId);
                }
            }
        }

        public void ChangeResourceType()
        {
            if (ChoosenResourceType == ResourceType)
                ChoosenResourceType = SecondResourceType;
            else if (ChoosenResourceType == SecondResourceType)
                ChoosenResourceType = ResourceType;

            MapUiManager.instance.UpdateChoosenResource(this);
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

        public Dictionary<string, int> GetCost()
        {
            Dictionary<string, int> BuyingCost = new Dictionary<string, int>();
            BuyingCost.Add("gold", BaseGoldCost);
            BuyingCost.Add("wood", BaseWoodCost);
            BuyingCost.Add("stone", BaseStoneCost);
            BuyingCost.Add("metal", BaseMetalCost);

            return BuyingCost;
        }

        public Dictionary<string, int> GetUpgradeCost()
        {
            Dictionary<string, int> BuyingCost = new Dictionary<string, int>();
            BuyingCost.Add("gold", (int)(UpgradeGoldCost + (Level * LevelModifier * 1.5)));
            BuyingCost.Add("wood", (int)(UpgradeWoodCost + (Level * LevelModifier * 1.5)));
            if (Level > 2)
                BuyingCost.Add("stone", (int)(UpgradeStoneCost + (Level * LevelModifier * 1.5)));
            if (Level > 4 && UnlockedMetal())
                BuyingCost.Add("metal", (int)(UpgradeMetalCost + (Level * LevelModifier * 1.5)));

            return BuyingCost;
        }

        public bool UnlockedMetal()
        {
            if (Level >= LevelToUnlockSecondResourceType)
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