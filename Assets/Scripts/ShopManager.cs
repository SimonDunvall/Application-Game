using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public CustomCursor customCursor;

    public void BuyBuilding(Building building)
    {
        SceneManager.LoadScene(0);

        customCursor.gameObject.SetActive(true);
        customCursor.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
        Cursor.visible = false;

        StaticClass.BoughtBuilding = building;
        StaticClass.CustomCursor = customCursor;
    }
}