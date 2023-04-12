using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Buildings;
using Assets.Scripts.Managers;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [System.Serializable]
    public class TileData : IDataPersistence
    {
        public string SUB { get => "/tile"; }
        public string COUNT_SUB { get => "/tile.count"; }

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
            formatter.Serialize(stream, new TileData((Tile)SaveSystemManager.tiles[i]));
        }

        public void LoadData(BinaryFormatter formatter, FileStream stream)
        {
            var data = (TileData)formatter.Deserialize(stream);

            Tile.FindTile(new Vector3(data.posistion[0], data.posistion[1], data.posistion[2])).isOccupied = true;
        }
    }
}