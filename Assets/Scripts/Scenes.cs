using System.Collections;
using System.Linq;
using Assets.Scripts.SaveSystem;
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
            foreach (var tile in SaveSystemManager.tiles)
            {
                Vector3 position = tile.transform.position;
                if (!StaticClass.TilesToSave.Contains(position))
                {
                    StaticClass.TilesToSave.Add(position);
                }
            };

            BuildingsSetActive(false);
            StartCoroutine(LoadSceneAsync(1));
        }

        private void BuildingsSetActive(bool isActive)
        {
            SaveSystemManager.mine.ForEach(m => m.gameObject.SetActive(isActive));
            SaveSystemManager.treeFarms.ForEach(m => m.gameObject.SetActive(isActive));
            SaveSystemManager.testBuildings.ForEach(m => m.gameObject.SetActive(isActive));
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