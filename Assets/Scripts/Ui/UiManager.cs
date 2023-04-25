using Assets.Scripts.SaveSystem;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Buildings;

public class UiManager : MonoBehaviour
{
    public GameObject storage;
    public GameObject timer;
    public Animator animatorStorage;
    public Animator animatorTimer;
    public TMP_Text innerStorageText;
    public TMP_Text timerText;
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

    public void OpenInspector(string storageText, int timeLeftText, string type, int instaceId)
    {
        BuildingId = instaceId;
        storage.SetActive(true);
        timer.SetActive(true);
        innerStorageText.text = $"{storageText} {type} \n(Collect)";
        timerText.text = $"{timeLeftText} Seconds";
        animatorStorage.SetTrigger("pop up");
        animatorTimer.SetTrigger("pop up");
    }

    public void CloseInspector()
    {
        storage.SetActive(false);
        timer.SetActive(false);
    }

    public void UpdateResourceText(string storageText, string type, int instaceId)
    {
        if (storage.activeSelf && BuildingId == instaceId)
            innerStorageText.text = $"{storageText} {type} \n(Collect)";
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
            timerText.text = $"{timeLeftText} Seconds";

    }
}
