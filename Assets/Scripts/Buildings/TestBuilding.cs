using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Map;
using Assets.Scripts.SaveSystem;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public class TestBuilding : MonoBehaviour, IBuilding
    {
        public int InstaceId => GetInstanceID();
        public string Type => "testBuilding";
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
        public float LevelModifier;

        private void Awake()
        {
            SaveSystemManager.testBuildings.Add(this);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void PlaceBuilding()
        {
            var customCursor = StaticClass.GetCustomCursor();
            var tiles = GameManager.instance.tiles;
            Tile nearestTile = Tile.GetNearestTile();

            List<Tile> neighbouringTiles = nearestTile.FindAllTileNeighbors();
            neighbouringTiles.Add(nearestTile);
            if (neighbouringTiles.Count == 9 && neighbouringTiles.All(tile => !tile.isOccupied))
            {
                Resources.Pay(GetCost());

                CreateObject(this, nearestTile.transform.position);
                nearestTile.SetCloseTilesOccupied();

                customCursor.DisableCursor();
                GameManager.instance.ResetValues();
            }
        }

        public static TestBuilding CreateObject(TestBuilding preFab, Vector3 posistion)
        {
            return Instantiate(preFab, posistion, Quaternion.identity);
        }

        public Sprite GetSprite()
        {
            return GetComponent<SpriteRenderer>().sprite;
        }

        public void LevelUp()
        {
            if (Level < MaxLevel)
            {
                Level += 1;
                MapUiManager.instance.UpdateLevelText(this);
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
            if (Level > 1)
                BuyingCost.Add("stone", (int)(UpgradeStoneCost + (Level * LevelModifier * 1.5)));
            if (Level > 3)
                BuyingCost.Add("metal", (int)(UpgradeMetalCost + (Level * LevelModifier * 1.5)));

            return BuyingCost;
        }
    }
}