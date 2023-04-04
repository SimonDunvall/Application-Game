using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private Building buildingToPlace;
    private CustomCursor customCursor;
    public Tile[] tiles;

    private void Start()
    {
        buildingToPlace = StaticClass.BoughtBuilding;
        customCursor = StaticClass.CustomCursor;
    }

    private void Update()
    {
        PlaceBuilding();
    }

    public void PlaceBuilding()
    {
        if (Input.GetMouseButton(0) && buildingToPlace != null)
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
                Instantiate(buildingToPlace, nearestTile.transform.position, Quaternion.identity);
                buildingToPlace = null;

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
