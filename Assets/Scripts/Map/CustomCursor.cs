using Assets.Scripts.Buildings;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class CustomCursor : MonoBehaviour
    {
        internal void DisableCursor()
        {
            gameObject.SetActive(false);
            Cursor.visible = true;
        }

        internal void UseCursor(IBuilding building)
        {
            gameObject.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = building.GetSprite();
            Cursor.visible = false;
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
    }
}