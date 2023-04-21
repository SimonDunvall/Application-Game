using Assets.Scripts.Buildings;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ShopManager : MonoBehaviour
    {
        public CustomCursor customCursor;

        public void BuyTestBuilding(TestBuilding building)
        {
            BuyBuilding(building);
        }

        public void BuyTreeFarm(TreeFarm building)
        {
            BuyBuilding(building);
        }

        public void BuyMine(Mine building)
        {
            BuyBuilding(building);
        }

        private void BuyBuilding(IBuilding building)
        {
            FindObjectOfType<Scenes>().LoadMap();

            customCursor.UseCursor(building);

            StaticClass.SetBoughtBuilding(building);
            StaticClass.SetCustomCursor(customCursor);
        }
    }
}