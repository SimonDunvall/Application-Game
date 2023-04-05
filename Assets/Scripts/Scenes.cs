using System.Collections;
using System.Collections.Generic;
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
            SceneManager.LoadScene(1);
        }
    }
}