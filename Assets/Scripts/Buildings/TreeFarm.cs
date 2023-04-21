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
    public float TimeLeft { get; set; }
    private float nextIncreaseTime = 60f;

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
        TimeLeft = nextIncreaseTime - Time.time;

        UpdateWood();
    }

    private void UpdateWood()
    {
        if (TimeLeft <= 0 && InnerStorage < InnerStorageSize)
        {
            nextIncreaseTime = Time.time + 60f;
            InnerStorage += WoodPerMinute;
            Debug.Log($"added wood to innerstorage {InnerStorage}");
        }
        if (InnerStorage > InnerStorageSize)
            InnerStorage = InnerStorageSize;
    }

    private void OnMouseDown()
    {
        if (InnerStorage > 0)
        {
            SaveSystemManager.resources.wood += InnerStorage;
            InnerStorage = 0;
        }
        else
        {
            Debug.Log($"innerstorage is: {InnerStorage}");
            Debug.Log($"Time to next payout is in: {TimeLeft}");
        }
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
