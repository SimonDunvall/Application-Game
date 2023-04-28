using Assets.Scripts.SaveSystem;
using TMPro;
using System.Linq;
using Assets.Scripts.Buildings;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject storage;
    public GameObject timer;
    public GameObject levelUp;
    public GameObject changeResourceType;
    public Animator animatorStorage;
    public Animator animatorTimer;
    public Animator animatorLevelUp;
    public Animator animatorChangeResourceType;
    public TMP_Text innerStorageText;
    public TMP_Text timerText;
    public TMP_Text levelUpText;
    public TMP_Text changeResourceTypeText;
    private static int BuildingId;

    public static UiManager instance { get; private set; }

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
            storage.SetActive(false);
            timer.SetActive(false);
            changeResourceType.SetActive(false);
        }
    }


    public void CloseInspector()
    {
        storage.SetActive(false);
        timer.SetActive(false);
        levelUp.SetActive(false);
        changeResourceType.SetActive(false);
    }

    public void UpdateResourceText(string storageText, string type, int instaceId)
    {
        if (storage.activeSelf && BuildingId == instaceId)
        {
            innerStorageText.text = $"{storageText} {type} \n(Collect)";
        }
    }

    public void CollectResource()
    {
        // Create dictionaries that map GetInstanceID() values to building objects
        var treeFarmDict = SaveSystemManager.treeFarms.ToDictionary(b => b.GetInstanceID(), b => b);
        var mineDict = SaveSystemManager.mine.ToDictionary(b => b.GetInstanceID(), b => b);

        // Iterate over the GetInstanceID() values for a given BuildingId
        foreach (var instanceId in treeFarmDict.Keys.Concat(mineDict.Keys).Distinct().Where(id => id == BuildingId))
        {
            // Look up the corresponding building object and call its CollectStorage() method
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
                break;
            }
        }
    }

    internal void UpdateLevelText(IBuilding building)
    {
        if (levelUp.activeSelf && BuildingId == building.GetInstanceID())
        {
            levelUpText.text = building.IsMaxLevel() ? $"Max Level \n Level {building.Level}" : $"Upgrade Cost: Free \n Level {building.Level}";

            Button buttonComponent = levelUp.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.interactable = !building.IsMaxLevel();
            }
        }
    }

    internal void UpdateChoosenResource(Mine mine)
    {
        if (changeResourceType.activeSelf)
            changeResourceTypeText.text = $"Choosen Resource {mine.ChoosenResourceType} \n Click to change";
    }
}
