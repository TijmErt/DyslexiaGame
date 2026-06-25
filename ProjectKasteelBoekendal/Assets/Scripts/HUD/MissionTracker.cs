using System;
using System.Collections.Generic;
using System.Linq;
using Managers.Quest;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class MissionTracker : MonoBehaviour
{
    public Transform objectiveList;
    public GameObject objectiveText;
    
    [SerializeField] private QuestMediator questMediator;
    
    private readonly List<GameObject> spawnedObjects = new();

    private void Awake()
    {
        questMediator = GameObject.FindFirstObjectByType<QuestMediator>();
    }

    private void Start()
    {
        if (questMediator == null)
        {
            Debug.LogError("questMediator is not assigned!");
            return;
        }

        questMediator.OnQuestChanged += RebuildUI; Debug.Log("Subscribed to OnQuestChanged event");
    }

    private void OnEnable()
    {
        if (questMediator == null)
        {
            Debug.LogError("questMediator is not assigned!");
            return;
        }

        questMediator.OnQuestChanged += RebuildUI; Debug.Log("Subscribed to OnQuestChanged event");

        if (spawnedObjects.Count == 0)
        {
            RebuildUI();
        }
    }

    private void OnDisable()
    {
        if (questMediator != null) questMediator.OnQuestChanged -= RebuildUI; Debug.Log("UnSubscribed to OnQuestChanged event");
    }
    
    private void RebuildUI()
    {
        Debug.Log("Rebuilding UI");
        ClearUI();
        
        foreach (QuestProgress questProgress in questMediator.GetQuestsByState(QuestEnums.QuestState.Active))
        {
            Debug.Log("Current Quest : " + questProgress.QuestInfo.QuestID);
            foreach (ObjectiveProgress objective in questProgress.Objectives)
            {
                Debug.Log("Objective : " + objective.Objective.TargetID);
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
            QuestTarget target = QuestTargetRegistry.Instance.Get(objectiveProgress.Objective.TargetID);

            if (target != null && SceneManager.GetActiveScene().name.Contains(objectiveProgress.Objective.SceneName))
            {
                Vector3 targetLocation = target.FocusQuestTarget(); //This can be used later to make a pointer towards the target.
            }
            else
            {
                //This can later contain a feature to look into what scene it can be found in. Only really useful if a quest requires backtracking or if player backtracks
                Debug.Log($"{objectiveProgress.Objective.TargetID} not found within Scene ["+ SceneManager.GetActiveScene().name +"]!");
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
