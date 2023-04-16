using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.SaveSystem;
using UnityEngine;

public class FileDataHandler
{
    private string baseUrl = "";

    public FileDataHandler(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }

    internal void Load(IDataPersistence dataType)
    {
        var path = baseUrl + dataType.SUB;
        var countPath = baseUrl + dataType.SUB;

        BinaryFormatter formatter = new BinaryFormatter();
        int count = 0;

        if (File.Exists(countPath))
        {
            using (FileStream countStream = new FileStream(countPath, FileMode.Open))
            {
                count = (int)formatter.Deserialize(countStream);
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (File.Exists(path + i))
            {
                try
                {
                    using (FileStream stream = new FileStream(path + i, FileMode.Open))
                    {
                        dataType.LoadData(formatter, stream);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error occured when trying to load save: {path} \n {ex}");
                }
            }
            else
            {
                Debug.LogError($"Path not found in {path + i}");
            }
        }
    }

    internal void Save(IDataPersistence dataType, int count)
    {
        var path = baseUrl + dataType.SUB;
        var countPath = baseUrl + dataType.SUB;

        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            using (FileStream countStream = new FileStream(countPath, FileMode.Create))
            {
                formatter.Serialize(countStream, count);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error occured when trying to save data to file: {path} \n {ex}");
        }

        for (int i = 0; i < count; i++)
        {
            using (FileStream stream = new FileStream(path + i, FileMode.Create))
            {
                dataType.SaveData(formatter, stream, i);
            };
        }
    }
}
