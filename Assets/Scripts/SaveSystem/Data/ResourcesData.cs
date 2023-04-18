using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.SaveSystem.Data
{
    [System.Serializable]
    public class ResourcesData : IDataPersistence
    {
        public string SUB { get => "/resources/resources"; }
        public string COUNT_SUB { get => "/resources/resources.count"; }

        public int gold;
        public int wood;
        public int stone;
        public int metal;

        public ResourcesData(Resources resources)
        {
            gold = resources.gold;
            wood = resources.wood;
            stone = resources.stone;
            metal = resources.metal;
        }

        public ResourcesData()
        {
        }

        public void LoadData(BinaryFormatter formatter, FileStream stream)
        {
            SaveSystemManager.resources = (ResourcesData)formatter.Deserialize(stream);
        }

        public void SaveData(BinaryFormatter formatter, FileStream stream, int i)
        {
            formatter.Serialize(stream, new ResourcesData(SaveSystemManager.resources));
        }
    }
}