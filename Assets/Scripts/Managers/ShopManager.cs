using Assets.Scripts.Buildings;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ShopManager : MonoBehaviour
    {
        public CustomCursor customCursor;

        public void BuyBuilding(IBuilding building)
        {
            FindObjectOfType<Scenes>().LoadMap();

            customCursor.UseCursor(building);

            StaticClass.SetBoughtBuilding(building);
            StaticClass.SetCustomCursor(customCursor);
        }
    }
}