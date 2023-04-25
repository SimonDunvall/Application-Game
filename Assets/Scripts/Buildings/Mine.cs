using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Buildings;
using Assets.Scripts.Managers;
using Assets.Scripts.Map;
using Assets.Scripts.SaveSystem;
using UnityEngine;

public class Mine : MonoBehaviour, IBuilding, IResourceBuilding
{
    public string Type { get => "Mine"; }

    public int ResourecePerMinue;
    public int InnerStorageSize;
    public int InnerStorage { get; set; }
    public float TimeLeft { get; set; }
    private float nextIncreaseTime = 60f;
    public string ResourceType { get => "Stone"; }

    private void Awake()
    {
        SaveSystemManager.mine.Add(this);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        UpdateTimer();
        UpdateResource();
    }

    private void UpdateTimer()
    {
        TimeLeft = nextIncreaseTime - Time.time;
        UiManager.instance.UpdateTimerText((int)TimeLeft, this.GetInstanceID());
    }

    private void UpdateResource()
    {
        if (TimeLeft <= 0 && InnerStorage < InnerStorageSize)
        {
            nextIncreaseTime = Time.time + 60f;
            InnerStorage += ResourecePerMinue;
            UiManager.instance.UpdateResourceText(InnerStorage.ToString(), ResourceType, this.GetInstanceID());

        }
        if (InnerStorage > InnerStorageSize)
            InnerStorage = InnerStorageSize;
    }

    public void CollectStorage()
    {
        if (InnerStorage > 0)
        {
            SaveSystemManager.resources.stone += InnerStorage;
            InnerStorage = 0;
            UiManager.instance.UpdateResourceText(InnerStorage.ToString(), ResourceType, this.GetInstanceID());
        }
    }

    public Sprite GetSprite()
    {
        return this.GetComponent<SpriteRenderer>().sprite;
    }

    public void PlaceBuilding()
    {
        Tile nearestTile = Tile.GetNearestTile();

        List<Tile> neighbouringTiles = nearestTile.FindAllTileNeighbors();
        neighbouringTiles.Add(nearestTile);
        if (neighbouringTiles.Count == 9 && neighbouringTiles.All(tile => tile.isOccupied == false))
        {
            CreateObject(this, nearestTile.transform.position);

            nearestTile.SetCloseTilesOccupied();
            StaticClass.GetCustomCursor().DisableCursor();
            GameManager.instance.ResetValues();
        }
    }

    public static Mine CreateObject(Mine preFab, Vector3 position)
    {
        return Instantiate(preFab, position, Quaternion.identity);
    }
}
