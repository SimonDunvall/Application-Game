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