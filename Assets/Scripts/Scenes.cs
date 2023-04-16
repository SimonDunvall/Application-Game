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
            SceneManager.LoadScene(0);
        }

        public void LoadShop()
        {
            Debug.Log("tiles to quick save " + SaveSystemManager.tiles.Count());
            SaveSystemManager.tiles.ForEach(tile => StaticClass.tilesToSave.Add(tile.transform.position));
            SceneManager.LoadScene(1);
        }
    }
}