using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DataPersistence.Data;
using UnityEngine;

namespace Assets.Scripts.DataPersistence
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);
        void SaveData(ref GameData data);
    }
}