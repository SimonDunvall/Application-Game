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
        public string Type => "testBuilding";

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
    }
}