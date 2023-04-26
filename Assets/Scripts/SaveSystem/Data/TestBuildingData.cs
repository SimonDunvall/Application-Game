using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Buildings;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [System.Serializable]
    public class TestBuildingData : IDataPersistence
    {
        public string SUB { get => "/buildings/testbuilding"; }
        public string COUNT_SUB { get => "/buildings/testbuilding.count"; }

        public string type;
        public int level;
        public float[] posistion = new float[3];

        public TestBuildingData(TestBuilding building)
        {
            type = building.Type;
            level = building.Level;

            posistion = new float[]
            {
                building.transform.position.x,
                building.transform.position.y,
                building.transform.position.z,
            };
        }

        public TestBuildingData()
        {
        }

        public void LoadData(BinaryFormatter formatter, FileStream stream)
        {
            var data = (TestBuildingData)formatter.Deserialize(stream);

            Vector3 posistion = new Vector3(data.posistion[0], data.posistion[1], data.posistion[2]);
            TestBuilding building = TestBuilding.CreateObject(SaveSystemManager.instance.testBuildingPrefab, posistion);

            building.Level = data.level;
        }

        public void SaveData(BinaryFormatter formatter, FileStream stream, int i)
        {
            formatter.Serialize(stream, new TestBuildingData(SaveSystemManager.testBuildings[i]));
        }
    }
}