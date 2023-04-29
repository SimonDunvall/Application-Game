using System.Collections;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.SaveSystem.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Scenes : MonoBehaviour
    {
        public void LoadMap()
        {
            StartCoroutine(LoadSceneAsync(0));
            BuildingsSetActive(true);
        }

        public void LoadShop()
        {
            TileData.quickSaveTiles();
            HomeData.quickSaveHomeLevel();

            BuildingsSetActive(false);
            StartCoroutine(LoadSceneAsync(1));
        }

        private void BuildingsSetActive(bool isActive)
        {
            SaveSystemManager.mine.ForEach(m => m.gameObject.SetActive(isActive));
            SaveSystemManager.treeFarms.ForEach(m => m.gameObject.SetActive(isActive));
        }

        IEnumerator LoadSceneAsync(int sceneIndex)
        {
            while (!SceneManager.LoadSceneAsync(sceneIndex).isDone)
            {
                yield return null;
            }
        }
    }
}