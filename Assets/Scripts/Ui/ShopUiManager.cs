using Assets.Scripts.Buildings;
using UnityEngine;
using UnityEngine.UI;
using Resources = Assets.Scripts.Resources;

public class ShopUiManager : MonoBehaviour
{
    public Button MineButton;
    public Mine Mine;
    public Button TreeFarmButton;
    public TreeFarm TreeFarm;

    public static ShopUiManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (!Resources.CanPay(Mine.GetCost()))
        {
            MineButton.interactable = false;
        }
        if (!Resources.CanPay(TreeFarm.GetCost()))
        {
            TreeFarmButton.interactable = false;
        }
    }
}
