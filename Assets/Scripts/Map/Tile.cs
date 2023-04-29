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
                MapUiManager.instance.CloseInspector();
        }

        internal static Tile FindTile(Vector3 posistion)
        {
            return GameManager.instance.tiles.FirstOrDefault(t => t.transform.position == posistion);
        }

        internal List<Tile> FindAllTileNeighbors(bool useLarge = false)
        {
            List<Tile> neighbouringTiles = new List<Tile>();

            foreach (Vector2 neighbourPosition in useLarge ? largeNeighbourPositions : neighbourPositions)
            {
                Vector3 position = (Vector2)this.transform.position + neighbourPosition;
                Tile neighbour = GameManager.instance.tiles.FirstOrDefault(tile => tile.transform.position == position);
                if (neighbour != null)
                {
                    neighbouringTiles.Add(neighbour);
                }
            }

            return neighbouringTiles;
        }

        internal void SetCloseTilesOccupied(bool useLarge = false)
        {
            var tiles = this.FindAllTileNeighbors(useLarge);
            tiles.Add(this);
            tiles.ForEach(tile => tile.isOccupied = true);
            SaveSystemManager.tiles.AddRange(tiles);
        }

        internal static Tile GetNearestTile(Vector3 postition)
        {
            Tile nearestTile = null;
            float shortestDistance = float.MaxValue;
            foreach (var tile in GameManager.instance.tiles)
            {
                float dist = Vector2.Distance(tile.transform.position, postition);
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

        private static readonly Vector2[] largeNeighbourPositions =
        {
            Vector2.up,
            Vector2.right,
            Vector2.down,
            Vector2.left,
            Vector2.up + Vector2.right,
            Vector2.up + Vector2.left,
            Vector2.down + Vector2.right,
            Vector2.down + Vector2.left,
            Vector2.up + Vector2.right + Vector2.up,
            Vector2.up + Vector2.left + Vector2.up,
            Vector2.down + Vector2.right + Vector2.down,
            Vector2.down + Vector2.left + Vector2.down,
            Vector2.up + Vector2.right + Vector2.right,
            Vector2.up + Vector2.left + Vector2.left,
            Vector2.down + Vector2.right + Vector2.right,
            Vector2.down + Vector2.left + Vector2.left,
            Vector2.up + Vector2.up,
            Vector2.right + Vector2.right,
            Vector2.down + Vector2.down,
            Vector2.left + Vector2.left,
            Vector2.up + Vector2.up + Vector2.right + Vector2.right,
            Vector2.up + Vector2.up + Vector2.left + Vector2.left,
            Vector2.down + Vector2.down + Vector2.right + Vector2.right,
            Vector2.down + Vector2.down + Vector2.left + Vector2.left
        };
    }
}