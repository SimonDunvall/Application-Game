using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Map;
using Assets.Scripts.SaveSystem;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public class Building : MonoBehaviour, IBuilding
    {
        private string _type = "testBuilding";
        public string Type { get => _type; set => _type = value; }
        private Vector3 _pos;
        public Vector3 Pos { get => _pos; set => _pos = value; }

        private void Awake()
        {
            SaveSystemManager.buildings.Add(this);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            SaveSystemManager.buildings.Remove(this);
        }

        public void PlaceBuilding(CustomCursor customCursor)
        {
            var tiles = GameManager.instance.tiles;
            Tile nearestTile = null;
            float shortestDistance = float.MaxValue;
            foreach (var tile in tiles)
            {
                float dist = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (dist < shortestDistance)
                {
                    shortestDistance = dist;
                    nearestTile = tile;
                }
            }

            List<Tile> allNeighbouringTiles = Tile.FindAllTileNeighbors(nearestTile.transform.position);
            if (nearestTile.isOccupied == false && allNeighbouringTiles.Count == 8 && allNeighbouringTiles.All(tile => tile.isOccupied == false))
            {
                CreateObject(this, nearestTile.transform.position);

                foreach (var tile in allNeighbouringTiles)
                {
                    tile.isOccupied = true;
                    SaveSystemManager.tiles.Add(tile);
                }
                nearestTile.isOccupied = true;
                SaveSystemManager.tiles.Add(nearestTile);
                StaticClass.BoughtBuilding = null;
                StaticClass.CustomCursor = null;
                customCursor.DisableCursor();
            }
        }

        public static Building CreateObject(Building preFab, Vector3 posistion)
        {
            return Instantiate(preFab, posistion, Quaternion.identity);
        }
    }
}