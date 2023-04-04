using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public CustomCursor customCursor;

    public void BuyBuilding(Building building)
    {
        FindObjectOfType<Scenes>().LoadMap();

        customCursor.UseCursor(building);

        StaticClass.BoughtBuilding = building;
        StaticClass.CustomCursor = customCursor;
    }
}