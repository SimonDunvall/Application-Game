using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Buildings;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [System.Serializable]
    public class MineData : IDataPersistence
    {
        public string SUB { get => "/buildings/mine"; }
        public string COUNT_SUB { get => "/buildings/mine.count"; }

        public string type;
        public int level;
        public float[] posistion = new float[3];

        public List<string> InnerStorage = new List<string>();

        public MineData(Mine building)
        {
            type = building.Type;
            level = building.Level;

            posistion = new float[]
            {
                building.transform.position.x,
                building.transform.position.y,
                building.transform.position.z,
            };

            InnerStorage = building.InnerStorage;
        }

        public MineData()
        {
        }

        public void LoadData(BinaryFormatter formatter, FileStream stream)
        {
            var data = (MineData)formatter.Deserialize(stream);

            Vector3 posistion = new Vector3(data.posistion[0], data.posistion[1], data.posistion[2]);
            Mine building = Mine.CreateObject(SaveSystemManager.instance.minePrefab, posistion);

            building.InnerStorage = data.InnerStorage;
            building.Level = data.level;
        }

        public void SaveData(BinaryFormatter formatter, FileStream stream, int i)
        {
            formatter.Serialize(stream, new MineData(SaveSystemManager.mine[i]));
        }
    }
}