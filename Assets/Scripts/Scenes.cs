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
            SaveSystemManager.tiles.ForEach(tile => StaticClass.GettilesToSave().Add(tile.transform.position));
            SceneManager.LoadScene(1);
        }
    }
}