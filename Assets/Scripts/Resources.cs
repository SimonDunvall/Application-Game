using Assets.Scripts.Managers;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.SaveSystem.Data;

namespace Assets.Scripts
{
    public class Resources
    {
        public int gold { get; set; }
        public int wood { get; set; }
        public int stone { get; set; }
        public int metal { get; set; }

        public static void UpdateResources()
        {
            GameManager.instance.goldDisplay.text = SaveSystemManager.resources.gold.ToString();
            GameManager.instance.woodDisplay.text = SaveSystemManager.resources.wood.ToString();
            GameManager.instance.stoneDisplay.text = SaveSystemManager.resources.stone.ToString();
            GameManager.instance.MetalDisplay.text = SaveSystemManager.resources.metal.ToString();
        }

        public static implicit operator Resources(ResourcesData r)
        {
            return new Resources()
            {
                gold = r.gold,
                wood = r.wood,
                stone = r.stone,
                metal = r.metal
            };
        }
    }
}