using Assets.Scripts.SaveSystem;
using TMPro;
using System.Linq;
using Assets.Scripts.Buildings;
using UnityEngine;
using Resources = Assets.Scripts.Resources;

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

    public void OpenInspector(string storageText, int timeLeftText, int levelText, string type, int instaceId, bool showChangeResourceType = false)
    {
        BuildingId = instaceId;
        storage.SetActive(true);
        timer.SetActive(true);
        levelUp.SetActive(true);
        innerStorageText.text = $"{storageText} {type} \n(Collect)";
        timerText.text = $"{timeLeftText} Seconds";
        levelUpText.text = $"Upgrade Cost: Free \n Level {levelText}";
        animatorStorage.SetTrigger("pop up");
        animatorTimer.SetTrigger("pop up");
        animatorLevelUp.SetTrigger("pop up");
        if (showChangeResourceType)
        {
            changeResourceType.SetActive(true);
            changeResourceTypeText.text = $"Choosen Resource {type} \n Click to change";
            animatorChangeResourceType.SetTrigger("pop up");
        }

        Resources.UpdateResources();
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
            Resources.UpdateResources();
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

    public void UpdateTimerText(int timeLeftText, int instaceId)
    {
        if (storage.activeSelf && BuildingId == instaceId)
            timerText.text = $"{timeLeftText} Seconds";
    }

    public void LevelUpBuidling()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Building"))
        {
            var building = item.GetComponent<IBuilding>();
            if (building.GetInstanceID() == BuildingId)
            {
                building.LevelUp();
                break;
            }
        }
    }

    internal void UpdateLevelText(int levelText, int instaceId)
    {
        if (levelUp.activeSelf && BuildingId == instaceId)
        {
            levelUpText.text = $"Upgrade Cost: Free \n Level {levelText}";
            Resources.UpdateResources();
        }
    }

    internal void UpdateChoosenResource(string choosenResourceType)
    {
        if (changeResourceType.activeSelf)
            changeResourceTypeText.text = $"Choosen Resource {choosenResourceType} \n Click to change";
    }
}
