using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Buildings;
using Assets.Scripts.Managers;
using Assets.Scripts.Map;
using Assets.Scripts.SaveSystem;
using UnityEngine;

public class TreeFarm : MonoBehaviour, IBuilding
{
    private string _type = "TreeFarm";
    public string Type { get => _type; set => _type = value; }
    private Vector3 _pos;
    public Vector3 Pos { get => _pos; set => _pos = value; }

    public int WoodPerMinute;
    public int InnerStorageSize;
    public int InnerStorage { get; set; }
    private float nextIncreaseTime;

    private void Awake()
    {
        SaveSystemManager.treeFarms.Add(this);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Time.time > nextIncreaseTime && InnerStorage < InnerStorageSize)
        {
            nextIncreaseTime = Time.time + 60;
            InnerStorage += WoodPerMinute;
        }
        if (InnerStorage > InnerStorageSize)
            InnerStorage = InnerStorageSize;
    }

    public Sprite GetSprite()
    {
        return this.GetComponent<SpriteRenderer>().sprite;
    }

    public void PlaceBuilding()
    {
        var customCursor = StaticClass.GetCustomCursor();
        var tiles = GameManager.instance.tiles;
        Tile nearestTile = Tile.GetNearestTile();

        List<Tile> neighbouringTiles = nearestTile.FindAllTileNeighbors();
        neighbouringTiles.Add(nearestTile);
        if (neighbouringTiles.Count == 9 && neighbouringTiles.All(tile => tile.isOccupied == false))
        {
            CreateObject(this, nearestTile.transform.position);

            nearestTile.SetCloseTilesOccupied();

            customCursor.DisableCursor();
            GameManager.instance.ResetValues();
        }
    }

    public static TreeFarm CreateObject(TreeFarm preFab, Vector3 position)
    {
        return Instantiate(preFab, position, Quaternion.identity);
    }
}
