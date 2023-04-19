using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [System.Serializable]
    public class TileData : IDataPersistence
    {
        public string SUB { get => "/tiles/tile"; }
        public string COUNT_SUB { get => "/tiles/tile.count"; }

        public float[] posistion = new float[3];

        public TileData(Tile tile)
        {
            posistion = new float[]
            {
                tile.transform.position.x,
                tile.transform.position.y,
                tile.transform.position.z,
            };
        }

        public TileData()
        {
        }

        public void SaveData(BinaryFormatter formatter, FileStream stream, int i)
        {
            formatter.Serialize(stream, new TileData(SaveSystemManager.tiles[i]));
        }

        public void LoadData(BinaryFormatter formatter, FileStream stream)
        {
            var data = (TileData)formatter.Deserialize(stream);
            Tile tile = Tile.FindTile(new Vector3(data.posistion[0], data.posistion[1], data.posistion[2]));

            tile.isOccupied = true;

            SaveSystemManager.tiles.Add(tile);
        }
    }
}