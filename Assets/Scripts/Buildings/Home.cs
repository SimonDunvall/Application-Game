using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Map;
using Assets.Scripts.SaveSystem;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Buildings
{
    public class Home : MonoBehaviour, IBuilding
    {
        public int InstaceId => GetInstanceID();
        public string Type => "home";
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
        public int UpgradeGoldCost;
        public int UpgradeWoodCost;
        public int UpgradeStoneCost;
        public int UpgradeMetalCost;
        public int GoldPerWood;
        public int GoldPerStone;
        public int GoldPerMetal;
        public Dictionary<string, (int, int)> GoldPerResources { get; set; } = new Dictionary<string, (int, int)>();
        public float LevelModifier;

        public static Home instance { get; private set; }

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
            if (SaveSystemManager.home.Count() == 0)
                SaveSystemManager.home.Add(this);
        }

        private void Start()
        {
            SetTilesToOccupied();
        }

        private void SetTilesToOccupied()
        {
            Tile nearestTile = Tile.GetNearestTile(new Vector3((float)-0.5, (float)-0.5, 0));
            if (!nearestTile.isOccupied)
            {
                List<Tile> neighbouringTiles = nearestTile.FindAllTileNeighbors(true);
                neighbouringTiles.Add(nearestTile);

                if (neighbouringTiles.Count() == 25 && neighbouringTiles.All(tile => !tile.isOccupied))
                {
                    nearestTile.SetCloseTilesOccupied(true);
                }
            }
        }

        public static Home CreateObject(Home preFab, Vector3 posistion)
        {
            throw new System.NotImplementedException();
        }

        public Sprite GetSprite()
        {
            return GetComponent<SpriteRenderer>().sprite;
        }

        public void LevelUp()
        {
            if (Level < MaxLevel)
            {
                Resources.Pay(GetUpgradeCost());
                Resources.UpdateResources();
                Level += 1;
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

        public void PlaceBuilding()
        {
            throw new System.NotImplementedException();
        }

        internal Dictionary<string, (int, int)> GetTradeConverstion()
        {
            Random rnd = new Random();
            Dictionary<string, (int, int)> goldPerResource = new Dictionary<string, (int, int)>()
            {
                { "wood", ((int)(GoldPerWood + (LevelModifier * rnd.Next((int)(LevelModifier * Level)))), rnd.Next(1000)) },
                { "stone", ((int)(GoldPerStone + (LevelModifier * rnd.Next((int)(LevelModifier * Level + Level)))) ,rnd.Next(1000)) },
                { "metal", ((int)(GoldPerMetal + Level + (LevelModifier * rnd.Next((int)(LevelModifier * Level)))), rnd.Next(1000)) }
            };

            GoldPerResources = goldPerResource;
            return goldPerResource;
        }

        public void SellWood()
        {
            var tradeInfo = Home.instance.GoldPerResources["wood"];
            var price = new Dictionary<string, int>() { { "wood", tradeInfo.Item2 } };
            if (Resources.CanPay(price))
            {
                Resources.Pay(price);
                SaveSystemManager.resources.gold += tradeInfo.Item1 * tradeInfo.Item2;
                Resources.UpdateResources();

                MapUiManager.instance.OpenInspector(Home.instance);
            }
        }


        public void SellStone()
        {
            var tradeInfo = Home.instance.GoldPerResources["stone"];
            var price = new Dictionary<string, int>() { { "stone", tradeInfo.Item2 } };
            if (Resources.CanPay(price))
            {
                Resources.Pay(price);
                SaveSystemManager.resources.gold += tradeInfo.Item1 * tradeInfo.Item2;
                Resources.UpdateResources();

                MapUiManager.instance.OpenInspector(Home.instance);
            }
        }

        public void SellMetal()
        {
            var tradeInfo = Home.instance.GoldPerResources["metal"];
            var price = new Dictionary<string, int>() { { "metal", tradeInfo.Item2 } };
            if (Resources.CanPay(price))
            {
                Resources.Pay(price);
                SaveSystemManager.resources.gold += tradeInfo.Item1 * tradeInfo.Item2;
                Resources.UpdateResources();

                MapUiManager.instance.OpenInspector(Home.instance);
            }
        }
    }
}