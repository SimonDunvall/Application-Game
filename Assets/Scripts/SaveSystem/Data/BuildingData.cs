using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Buildings;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [System.Serializable]
    public class BuildingData : IDataPersistence
    {
        public string SUB { get => "/building"; }
        public string COUNT_SUB { get => "/building.count"; }

        public string type;
        public float[] posistion = new float[3];

        public BuildingData(Building building)
        {
            type = building.Type;

            posistion = new float[]
            {
                building.transform.position.x,
                building.transform.position.y,
                building.transform.position.z,
            };
        }

        public BuildingData()
        {
        }

        public void SaveData(BinaryFormatter formatter, FileStream stream, int i)
        {
            formatter.Serialize(stream, new BuildingData((Building)SaveSystemManager.buildings[i]));
        }

        public void LoadData(BinaryFormatter formatter, FileStream stream)
        {
            var data = (BuildingData)formatter.Deserialize(stream);

            Vector3 posistion = new Vector3(data.posistion[0], data.posistion[1], data.posistion[2]);
            Building building = Building.CreateObject(SaveSystemManager.instance.buildingPrefab, posistion);

            building.Type = this.type;
        }
    }
}