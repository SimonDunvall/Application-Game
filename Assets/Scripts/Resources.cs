using System;
using System.Collections.Generic;
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

        internal static bool CanPay(Dictionary<string, int> cost)
        {
            Pay(cost);

            if (SaveSystemManager.resources.gold < 0 || SaveSystemManager.resources.wood < 0 || SaveSystemManager.resources.stone < 0 || SaveSystemManager.resources.metal < 0)
            {
                UndoPayMent(cost);
                return false;
            }
            else
            {
                UndoPayMent(cost);
                return true;
            }
        }

        private static void UndoPayMent(Dictionary<string, int> cost)
        {
            if (cost.ContainsKey("gold"))
            {
                SaveSystemManager.resources.gold += cost["gold"];
            }
            if (cost.ContainsKey("wood"))
            {
                SaveSystemManager.resources.wood += cost["wood"];
            }
            if (cost.ContainsKey("stone"))
            {
                SaveSystemManager.resources.stone += cost["stone"];
            }
            if (cost.ContainsKey("metal"))
            {
                SaveSystemManager.resources.metal += cost["metal"];
            }


        }

        internal static void Pay(Dictionary<string, int> cost)
        {
            if (cost.ContainsKey("gold"))
            {
                SaveSystemManager.resources.gold -= cost["gold"];
            }
            if (cost.ContainsKey("wood"))
            {
                SaveSystemManager.resources.wood -= cost["wood"];
            }
            if (cost.ContainsKey("stone"))
            {
                SaveSystemManager.resources.stone -= cost["stone"];
            }
            if (cost.ContainsKey("metal"))
            {
                SaveSystemManager.resources.metal -= cost["metal"];
            }

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