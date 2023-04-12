using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Buildings;

namespace Assets.Scripts.SaveSystem
{
    internal interface IDataPersistence
    {
        string SUB { get; }
        string COUNT_SUB { get; }

        void LoadData(BinaryFormatter formatter, FileStream stream);
        void SaveData(BinaryFormatter formatter, FileStream stream, int i);
    }
}