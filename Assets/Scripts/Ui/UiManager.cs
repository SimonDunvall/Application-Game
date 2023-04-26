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
    public Animator animatorStorage;
    public Animator animatorTimer;
    public Animator animatorLevelUp;
    public TMP_Text innerStorageText;
    public TMP_Text timerText;
    public TMP_Text levelUpText;
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

    public void OpenInspector(string storageText, int timeLeftText, int levelText, string type, int instaceId)
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

        Resources.UpdateResources();
    }

    public void CloseInspector()
    {
        storage.SetActive(false);
        timer.SetActive(false);
        levelUp.SetActive(false);
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

    public void UpdateTimerText(int timeLeftText, int instaceId)
    {
        if (BuildingId == instaceId)
        {
            timerText.text = $"{timeLeftText} Seconds";
            Resources.UpdateResources();
        }
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
}
