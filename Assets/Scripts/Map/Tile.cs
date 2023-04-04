using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tile : MonoBehaviour
{
    public bool isOccupied;

    internal static List<Tile> FindAllTileNeighbors(Vector2 tilePosition, Tile[] tiles)
    {
        List<Tile> allNeighbouringTiles = new List<Tile>();
        foreach (Vector2 neighbourPosition in neighbourPositions)
        {
            Vector3 position = tilePosition + neighbourPosition;
            allNeighbouringTiles.AddRange(tiles.Where(tile => tile.transform.position == position));
        }
        return allNeighbouringTiles;
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
