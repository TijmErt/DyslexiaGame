using System;
using System.Collections.Generic;
using System.Linq;
using Managers.Quest;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MissionTracker : MonoBehaviour
{
    public Transform objectiveList;
    public GameObject objectiveText;
    
    [SerializeField] private QuestMediator questMediator;
    
    private readonly List<GameObject> spawnedObjects = new();
    private void Start()
    {
        questMediator = GameObject.FindFirstObjectByType<QuestMediator>();
        
        if (questMediator == null)
        {
            Debug.LogError("questMediator is not assigned!");
            return;
        }

        questMediator.OnQuestChanged += RebuildUI;

        if (spawnedObjects.Count == 0)
        {
            RebuildUI();
        }
    }
    private void OnDisable()
    {
        if (questMediator != null) questMediator.OnQuestChanged -= RebuildUI;
    }
    
    private void RebuildUI()
    {
        Debug.Log("Rebuilding UI");
        ClearUI();
        
        foreach (QuestProgress questProgress in questMediator.GetQuestsByState(QuestEnums.QuestState.Active))
        {
            foreach (ObjectiveProgress objective in questProgress.Objectives)
            {
                CreatObjectiveUI(objective);
            }
        }
    }
    

    private void CreatObjectiveUI(ObjectiveProgress objectiveProgress)
    { 
        GameObject objEntry = Instantiate(objectiveText, objectiveList);

        TextMeshProUGUI objEntryText = objEntry.GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault();
        if (objectiveProgress.IsCompleted)
        {
            objEntryText.text = $"<s>{objectiveProgress.Objective.Description} ({objectiveProgress.CurrentAmount}/{objectiveProgress.Objective.RequiredAmount}) </s>";
        }
        else
        {
            objEntryText.text = $"{objectiveProgress.Objective.Description} ({objectiveProgress.CurrentAmount}/{objectiveProgress.Objective.RequiredAmount})";
        }
        
        
        spawnedObjects.Add(objEntry);
        
        if (!string.IsNullOrEmpty(objectiveProgress.Objective.TargetID))
        {
            var target = QuestTargetRegistry.Instance.Get(objectiveProgress.Objective.TargetID);

            if (target != null)
            {
                target.FocusQuestTarget();
            }
            else
            {
                //This can later contain a feature to look into what scene it can be found in. Only really useful if a quest requires backtracking or if player backtracks
                Debug.LogError($"{objectiveProgress.Objective.TargetID} not found within Scene!");
            }
        }
    }
    
    private void ClearUI()
    {
        foreach (GameObject spawnedObject in spawnedObjects)
            Destroy(spawnedObject);

        spawnedObjects.Clear();
    }
    
    
    
}
