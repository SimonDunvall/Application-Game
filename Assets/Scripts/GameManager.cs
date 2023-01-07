using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private Building buildingToPlace;

    public CustomCursor customCursor;

    public Tile[] tiles;

    private void Update()
    {
        PlaceBuilding();
    }

    public void BuyBuilding(Building building)
    {
        customCursor.gameObject.SetActive(true);
        customCursor.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
        Cursor.visible = false;

        buildingToPlace = building;
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

            List<Tile> allNeighbouringTiles = FindAllTileNeighbors(nearestTile.transform.position);
            if (nearestTile.isOccupied == false && allNeighbouringTiles.Count == 8 && allNeighbouringTiles.All(tile => tile.isOccupied == false))
            {
                Instantiate(buildingToPlace, nearestTile.transform.position, Quaternion.identity);
                buildingToPlace = null;

                foreach (Tile tile in allNeighbouringTiles)
                {
                    tile.isOccupied = true;
                }
                nearestTile.isOccupied = true;
                customCursor.gameObject.SetActive(false);
                Cursor.visible = true;
            }
        }
    }

    private readonly Vector2[] neighbourPositions =
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

    public List<Tile> FindAllTileNeighbors(Vector2 tilePosition)
    {
        List<Tile> allNeighbouringTiles = new List<Tile>();
        foreach (Vector2 neighbourPosition in neighbourPositions)
        {
            Vector3 position = tilePosition + neighbourPosition;
            foreach (Tile tile in tiles)
            {
                if (tile.transform.position == position)
                {
                    allNeighbouringTiles.Add(tile);
                }
            }
        }
        return allNeighbouringTiles;
    }
}
