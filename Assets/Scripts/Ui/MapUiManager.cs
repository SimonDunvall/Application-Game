using Assets.Scripts.SaveSystem;
using TMPro;
using System.Linq;
using Assets.Scripts.Buildings;
using UnityEngine.UI;
using UnityEngine;
using Resources = Assets.Scripts.Resources;
using System.Collections.Generic;
using Random = System.Random;

public class MapUiManager : MonoBehaviour
{
    public GameObject storage;
    public GameObject timer;
    public GameObject levelUp;
    public GameObject changeResourceType;

    public GameObject woodTrade;
    public GameObject stoneTrade;
    public GameObject metalTrade;

    public Animator animatorStorage;
    public Animator animatorTimer;
    public Animator animatorLevelUp;
    public Animator animatorChangeResourceType;

    public Animator animatorWoodTrade;
    public Animator animatorStoneTrade;
    public Animator animatorMetalTrade;

    public TMP_Text innerStorageText;
    public TMP_Text timerText;
    public TMP_Text levelUpText;
    public TMP_Text changeResourceTypeText;

    public TMP_Text woodTradeText;
    public TMP_Text stoneTradeText;
    public TMP_Text metalTradeText;

    private static int BuildingId;

    public static MapUiManager instance { get; private set; }

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

    public void OpenInspector(IBuilding building, bool isResourceBuilding = false, string storageAmount = "", int timeLeft = 0, string resourceTypeText = "", bool showChangeResourceType = false)
    {
        BuildingId = building.GetInstanceID();
        CloseInspector();
        levelUp.SetActive(true);
        UpdateLevelText(building);
        animatorLevelUp.SetTrigger("pop up");

        if (isResourceBuilding)
        {
            storage.SetActive(true);
            timer.SetActive(true);
            UpdateResourceText(storageAmount, resourceTypeText, BuildingId);
            UpdateTimerText(BuildingId, false, timeLeft);
            animatorStorage.SetTrigger("pop up");
            animatorTimer.SetTrigger("pop up");

            changeResourceType.SetActive(showChangeResourceType);
            if (showChangeResourceType)
            {
                UpdateChoosenResource((Mine)building);
                animatorChangeResourceType.SetTrigger("pop up");
            }
        }
        else
        {
            woodTrade.SetActive(true);
            stoneTrade.SetActive(true);
            metalTrade.SetActive(true);
            var tradeInfo = Home.instance.GetTradeConverstion();
            UpdateTradeText("wood", tradeInfo);
            UpdateTradeText("stone", tradeInfo);
            UpdateTradeText("metal", tradeInfo);
            animatorWoodTrade.SetTrigger("pop up");
            animatorStoneTrade.SetTrigger("pop up");
            animatorMetalTrade.SetTrigger("pop up");
        }
    }

    public void CloseInspector()
    {
        storage.SetActive(false);
        timer.SetActive(false);
        levelUp.SetActive(false);
        changeResourceType.SetActive(false);

        woodTrade.SetActive(false);
        stoneTrade.SetActive(false);
        metalTrade.SetActive(false);
    }

    public void UpdateTradeText(string resource, Dictionary<string, (int, int)> tradeInfo)
    {
        if (woodTrade.activeSelf && stoneTrade.activeSelf && metalTrade.activeSelf)
        {
            var amount = tradeInfo[resource].Item2;
            var tradeText = $"{tradeInfo[resource].Item1 * amount} gold \n for \n {amount} {resource}";
            var tradeObject = Resources.CanPay(new Dictionary<string, int>() { { resource, amount } });

            switch (resource)
            {
                case "wood":
                    woodTradeText.text = tradeText;
                    woodTrade.GetComponent<Button>().interactable = tradeObject;
                    break;
                case "stone":
                    stoneTradeText.text = tradeText;
                    stoneTrade.GetComponent<Button>().interactable = tradeObject;
                    break;
                case "metal":
                    metalTradeText.text = tradeText;
                    metalTrade.GetComponent<Button>().interactable = tradeObject;
                    break;
            }
        }
    }

    public void UpdateResourceText(string storageText, string type, int instaceId)
    {
        if (storage && storage.activeSelf && BuildingId == instaceId)
        {
            innerStorageText.text = $"{storageText} {type} \n(Collect)";
        }
    }

    public void CollectResource()
    {
        var treeFarmDict = SaveSystemManager.treeFarms.ToDictionary(b => b.GetInstanceID(), b => b);
        var mineDict = SaveSystemManager.mine.ToDictionary(b => b.GetInstanceID(), b => b);

        foreach (var instanceId in treeFarmDict.Keys.Concat(mineDict.Keys).Distinct().Where(id => id == BuildingId))
        {
            if (treeFarmDict.TryGetValue(instanceId, out var treeFarm))
                treeFarm.CollectStorage();
            if (mineDict.TryGetValue(instanceId, out var mine))
                mine.CollectStorage();
        }
    }

    public void ChangeResourceTypeButton()
    {
        var mineDict = SaveSystemManager.mine.ToDictionary(b => b.GetInstanceID(), b => b);

        foreach (var instanceId in mineDict.Keys.Where(id => id == BuildingId))
        {
            if (mineDict.TryGetValue(instanceId, out var mine))
                mine.ChangeResourceType();
        }
    }

    public void UpdateTimerText(int instaceId, bool storageIsFull, int timeLeftText = 0)
    {
        if (timer && timer.activeSelf && BuildingId == instaceId && storageIsFull == false)
        {
            timerText.text = $"{timeLeftText} Seconds";
        }
        else if (timer && timer.activeSelf && BuildingId == instaceId)
        {
            timerText.text = "Storage Is Full";
        }
    }

    public void LevelUpBuidling()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Building"))
        {
            var building = item.GetComponent<IBuilding>();
            if (building.GetInstanceID() == BuildingId && building.Level < building.GetMaxLevel())
            {
                building.LevelUp();
                UpdateLevelText(building);

                var tradeInfo = Home.instance.GetTradeConverstion();
                UpdateTradeText("wood", tradeInfo);
                UpdateTradeText("stone", tradeInfo);
                UpdateTradeText("metal", tradeInfo);
                break;
            }
        }
    }

    internal void UpdateLevelText(IBuilding building)
    {
        if (levelUp.activeSelf && BuildingId == building.GetInstanceID())
        {
            string result = "";
            foreach (KeyValuePair<string, int> pair in building.GetUpgradeCost())
            {
                if (pair.Value != 0)
                {
                    result += pair.Key + ": " + pair.Value.ToString() + ", ";
                }
            }
            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - 2);
            }
            else
            {
                result = "Free";
            }

            levelUpText.text = building.IsMaxLevel() ? $"Max Level \n Level {building.Level}" : $"Upgrade Cost: {result} \n Level {building.Level}";

            Button buttonComponent = levelUp.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.interactable = !building.IsMaxLevel();
                buttonComponent.interactable = Resources.CanPay(building.GetUpgradeCost());
            }
        }
    }

    internal void UpdateChoosenResource(Mine mine)
    {
        if (changeResourceType.activeSelf)
            changeResourceTypeText.text = $"Choosen Resource {mine.ChoosenResourceType} \n Click to change";
    }
}
