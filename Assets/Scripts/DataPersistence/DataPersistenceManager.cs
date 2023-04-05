using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.DataPersistence.Data;

namespace Assets.Scripts.DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]
        [SerializeField] private string fileName;
        [SerializeField] private bool useEncryption;

        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;
        private FileDataHandler dataHandler;

        public static DataPersistenceManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
                Debug.LogError("Found more than one Data Persistence Manager in the scene");

            instance = this;
        }

        private void Start()
        {
            dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            dataPersistenceObjects = FindAllDataPersistneceObjects();
            LoadGame();
        }

        private void NewGame()
        {
            gameData = new GameData();
        }

        private void LoadGame()
        {
            gameData = dataHandler.Load();

            if (gameData == null)
            {
                Debug.Log("No save was found. Initializing data to defaults");
                NewGame();
            }

            foreach (var dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
        }

        private void SaveGame()
        {
            foreach (var dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref gameData);
            }

            dataHandler.Save(gameData);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<IDataPersistence> FindAllDataPersistneceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
                .OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    }
}