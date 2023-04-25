using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [System.Serializable]
    public class TreeFarmData : IDataPersistence
    {
        public string SUB { get => "/buildings/treefarm"; }
        public string COUNT_SUB { get => "/buildings/treefarm.count"; }

        public string type;
        public float[] posistion = new float[3];

        public int InnerStorage;

        public TreeFarmData(TreeFarm building)
        {
            type = building.Type;

            posistion = new float[]
            {
                building.transform.position.x,
                building.transform.position.y,
                building.transform.position.z,
            };

            InnerStorage = building.InnerStorage;
        }

        public TreeFarmData()
        {
        }

        public void LoadData(BinaryFormatter formatter, FileStream stream)
        {
            var data = (TreeFarmData)formatter.Deserialize(stream);

            Vector3 posistion = new Vector3(data.posistion[0], data.posistion[1], data.posistion[2]);
            TreeFarm building = TreeFarm.CreateObject(SaveSystemManager.instance.treeFarmPrefab, posistion);

            building.InnerStorage = data.InnerStorage;
        }

        public void SaveData(BinaryFormatter formatter, FileStream stream, int i)
        {
            formatter.Serialize(stream, new TreeFarmData(SaveSystemManager.treeFarms[i]));
        }
    }
}