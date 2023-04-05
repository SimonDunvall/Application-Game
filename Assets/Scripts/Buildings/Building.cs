using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    public class Building : MonoBehaviour, IBuilding
    {
        void IBuilding.PlaceBuilding(CustomCursor customCursor, Tile[] tiles)
        {
            if (Input.GetMouseButton(0))
            {
                Tile nearestTile = null;
                float shortestDistance = float.MaxValue;
                foreach (Tile tile in tiles)
                {
                    float dist = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if (dist < shortestDistance)
                    {
                        shortestDistance = dist;
                        nearestTile = tile;
                    }
                }

                List<Tile> allNeighbouringTiles = Tile.FindAllTileNeighbors(nearestTile.transform.position, tiles);
                if (nearestTile.isOccupied == false && allNeighbouringTiles.Count == 8 && allNeighbouringTiles.All(tile => tile.isOccupied == false))
                {
                    Instantiate(this, nearestTile.transform.position, Quaternion.identity);

                    foreach (var tile in allNeighbouringTiles)
                    {
                        tile.isOccupied = true;
                    }
                    nearestTile.isOccupied = true;
                    StaticClass.BoughtBuilding = null;
                    StaticClass.CustomCursor = null;
                    customCursor.DisableCursor();
                }
            }
        }
    }
}