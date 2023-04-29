using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Map;
using Assets.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Buildings
{
    public class Home : MonoBehaviour, IBuilding
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
        public int UpgradeGoldCost;
        public int UpgradeWoodCost;
        public int UpgradeStoneCost;
        public int UpgradeMetalCost;
        public float LevelModifier;

        static bool hasLoadedSave = false;

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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!hasLoadedSave)
            {
                hasLoadedSave = true;
                SetTilesToOccupied();
                SaveSystemManager.home.Add(this);
            }
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
    }
}