using Assets.Scripts.Buildings;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Managers
{
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
}