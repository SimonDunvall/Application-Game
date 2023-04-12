using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Map
{
    public class Tile : MonoBehaviour
    {
        public bool isOccupied;

        private void OnDestroy()
        {
            SaveSystemManager.tiles.Remove(this);
        }

        internal static List<Tile> FindAllTileNeighbors(Vector2 tilePosition)
        {
            List<Tile> allNeighbouringTiles = new List<Tile>();
            foreach (Vector2 neighbourPosition in neighbourPositions)
            {
                Vector3 position = tilePosition + neighbourPosition;
                allNeighbouringTiles.AddRange(GameManager.instance.tiles.Where(tile => tile.transform.position == position));
            }
            return allNeighbouringTiles;
        }

        internal static Tile FindTile(Vector3 posistion)
        {
            return GameManager.instance.tiles.FirstOrDefault(t => t.transform.position == posistion);
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