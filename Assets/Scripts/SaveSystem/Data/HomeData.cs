using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Buildings;
using UnityEngine;

namespace Assets.Scripts.SaveSystem.Data
{
    [System.Serializable]
    public class HomeData : IDataPersistence
    {
        public string SUB { get => "/buildings/home"; }
        public string COUNT_SUB { get => "/buildings/home.count"; }

        public string type;
        public int level;

        public HomeData(Home building)
        {
            type = building.Type;
            level = building.Level;
        }

        public HomeData()
        {
        }

        public void LoadData(BinaryFormatter formatter, FileStream stream)
        {
            var data = (HomeData)formatter.Deserialize(stream);
            var building = GameObject.Find("Home").GetComponent<Home>();
            building.Level = data.level;
        }

        public void SaveData(BinaryFormatter formatter, FileStream stream, int i)
        {
            formatter.Serialize(stream, new HomeData(SaveSystemManager.home[i]));
        }

        internal static void quickLoadHomeLevel()
        {
            GameObject.Find("Home").GetComponent<Home>().Level = StaticClass.HomeLevel;
        }

        internal static void quickSaveHomeLevel()
        {
            StaticClass.HomeLevel = GameObject.Find("Home").GetComponent<Home>().Level;
        }
    }
}