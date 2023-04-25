using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.Managers;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Map
{
    public class Tile : MonoBehaviour
    {
        public bool isOccupied;

        private void OnDestroy()
        {
            SaveSystemManager.tiles.Remove(this);
        }

        private void OnMouseDown()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                UiManager.instance.CloseInspector();
        }

        internal static Tile FindTile(Vector3 posistion)
        {
            return GameManager.instance.tiles.FirstOrDefault(t => t.transform.position == posistion);
        }

        internal List<Tile> FindAllTileNeighbors()
        {
            List<Tile> neighbouringTiles = new List<Tile>();
            foreach (Vector2 neighbourPosition in neighbourPositions)
            {
                Vector3 position = (Vector2)this.transform.position + neighbourPosition;
                neighbouringTiles.AddRange(GameManager.instance.tiles.Where(tile => tile.transform.position == position));
            }
            return neighbouringTiles;
        }

        internal void SetCloseTilesOccupied()
        {
            var tiles = this.FindAllTileNeighbors();
            tiles.Add(this);
            tiles.ForEach(tile => tile.isOccupied = true);
            SaveSystemManager.tiles.AddRange(tiles);
        }

        internal static Tile GetNearestTile()
        {
            Tile nearestTile = null;
            float shortestDistance = float.MaxValue;
            foreach (var tile in GameManager.instance.tiles)
            {
                float dist = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (dist < shortestDistance)
                {
                    shortestDistance = dist;
                    nearestTile = tile;
                }
            }
            return nearestTile;
        }

        private static readonly Vector2[] neighbourPositions =
        {
            Vector2.up,
            Vector2.right,
            Vector2.down,
            Vector2.left,
            Vector2.up + Vector2.right,
            Vector2.up + Vector2.left,
            Vector2.down + Vector2.right,
            Vector2.down + Vector2.left
        };
    }
}