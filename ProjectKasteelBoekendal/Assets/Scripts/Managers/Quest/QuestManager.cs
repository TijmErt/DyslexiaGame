using System;
using System.Collections.Generic;
using System.Linq;
using Managers.Saving;
using UnityEngine;

namespace Managers.Quest
{
    /// <summary>
    /// Manages quest initialization, progression, activation, completion,
    /// and persistence. Quests are loaded from resources and can be unlocked
    /// through event flags.
    /// </summary>
    public class QuestManager : MonoBehaviour, ISaveable
    {
        public static QuestManager instance;
        public string UID => "QuestManager";

        [SerializeField] private List<QuestProgress> Quests = new List<QuestProgress>();
        private Dictionary<string, QuestProgress> QuestsDictionary = new Dictionary<string, QuestProgress>();
        
        public List<QuestProgress> _activeQuests = new();
        
        public event Action OnQuestChanged; // Main usage is for other scripts to detect when the quest manager does something.

        private bool QuestChanged = false;
        private void Awake()
        {
            instance = this;
            Initialize();
        }

        private void Start()
        {
            EventFlagManager.instance.OnFlagChanged += HandleFlagChanged;
            
        }

        /// <summary>
        /// Loads all quest definitions, creates their runtime progress data,
        /// and determines which quests should be available at startup.
        /// </summary>
        private void Initialize()
        {
            List<QuestInfoSO> _allQuests =
                Resources.LoadAll<QuestInfoSO>("Quest")
                    .ToList();

            foreach (QuestInfoSO questInfo in _allQuests)
            {
                QuestProgress progress =
                    new QuestProgress(questInfo);

                Quests.Add(progress);

                QuestsDictionary.Add(
                    questInfo.QuestID,
                    progress);
            }

            CheckAvailability();
        }

        #region Action EventFlag
        //The purpose of this region is to listen in to changes in the EventFlagManager, this is to ensure that if a outside force triggers a flag, the quest associated to that flag will also become available.
        
        /// <summary>
        /// Subscribes to EventFlagManager events so quest availability can be
        /// re-evaluated when event flags change.
        /// </summary>
        private void OnEnable()
        {
           
        }
        
        /// <summary>
        /// Unsubscribes from EventFlagManager events.
        /// </summary>
        private void OnDisable()
        {
            if (EventFlagManager.instance != null) 
                EventFlagManager.instance.OnFlagChanged -= HandleFlagChanged;
        }
        
        /// <summary>
        /// Handles event flag changes by checking whether any quests have become available.
        /// </summary>
        /// <param name="flagName">The name of the flag that changed.</param>
        /// <param name="enabled">The new state of the flag.</param>
        private void HandleFlagChanged(string flagName, bool enabled)
        {
            CheckAvailability();
        }
        #endregion
        
        /// <summary>
        /// Evaluates all inactive quests and activates those whose requirements
        /// have been met. If the active quest list changes, listeners are notified.
        /// </summary>
        private void CheckAvailability()
        {
            foreach (QuestProgress quest in Quests)
            {
                if (quest.QuestState != QuestEnums.QuestState.Inactive) // Will only continue further if the quest isn't active or completed.
                    continue;

                if (string.IsNullOrEmpty(quest.QuestInfo.RequiredEventFlag)) //Will Activate the quest if has no activation Requirement
                {
                    ActivateQuest(quest.QuestInfo.QuestID);
                    continue;
                }

                if (EventFlagManager.instance.IsFlagEnabled(quest.QuestInfo.RequiredEventFlag)) // Will activate the quest if the activation requirement has been met
                {
                    ActivateQuest(quest.QuestInfo.QuestID);
                }
            }
            
            if(QuestChanged) OnQuestChanged?.Invoke(); // This is done so that after the Quest Manager has updated which quests are active, other scripts that rely on it will be notified of it
            QuestChanged = false;
        }

        /// <summary>
        /// Marks a quest as completed and triggers its completion event flag,
        /// if one has been configured.
        /// </summary>
        /// <param name="questID">The ID of the quest to complete.</param>
        private void CompleteQuest(string questID)
        {
            if (!QuestsDictionary.TryGetValue(questID, out QuestProgress quest)) //Checks whether Quest with ID exists
                return;

            if (quest.QuestState == QuestEnums.QuestState.Completed) //Checks if the Quest hasn't been completed before yet
                return;

            quest.QuestState = QuestEnums.QuestState.Completed; 
            
            if (!string.IsNullOrEmpty(quest.QuestInfo.CompletionEventFlag)) // Checks whether this quests triggers a EventFlag, if it does it sets this to true.
            {
                EventFlagManager.instance.ChangeFlagState(
                    quest.QuestInfo.CompletionEventFlag,true);
            }
            QuestChanged = true;
        }

        /// <summary>
        /// Activates a quest and adds it to the list of active quests.
        /// </summary>
        /// <param name="questID">The ID of the quest to activate.</param>
        private void ActivateQuest(string questID)
        {
            if (!QuestsDictionary.TryGetValue(questID, out QuestProgress quest))  //Checks whether Quest with ID exists
                return;

            if (quest.QuestState != QuestEnums.QuestState.Inactive) //Checks if the Quest is currently inactive
                return;

            quest.QuestState = QuestEnums.QuestState.Active;
            _activeQuests.Add(quest);
            QuestChanged = true;
        }

        /// <summary>
        /// Reports progress to all currently active quests. Any objectives that match
        /// the provided ObjectiveType and TargetID will be advanced by the given amount.
        /// If a quest becomes completed after the update, it is marked as completed and
        /// removed from the active quest list. Afterwards, the system checks if any new
        /// quests have become available.
        /// </summary>
        /// <param name="objectiveType">  The type of objective that generated the progress update. </param>
        /// <param name="targetID">
        /// The ID of the target associated with the progress update.
        /// Only objectives with a matching TargetID can be advanced.
        /// </param>
        /// <param name="amount">The amount of progress to add to matching objectives.</param>
        public void ReportProgress(QuestEnums.ObjectiveType objectiveType, string targetID, int amount)
        {
            List<QuestProgress> completedQuests = new();
            foreach (QuestProgress quest in _activeQuests)
            {
                bool changed = quest.TryAdvanceObjective(
                    objectiveType,
                    targetID,
                    amount);
   
                if(changed) QuestChanged = true;
                
                if (quest.CheckCompletion())
                {
                    completedQuests.Add(quest);
                }
            }

            foreach (QuestProgress quest in completedQuests)
            {
                CompleteQuest(quest.QuestInfo.QuestID);
                _activeQuests.Remove(quest);
            }
            CheckAvailability();
        }

        /// <summary>
        /// Retrieves a quest by its unique identifier.
        /// </summary>
        /// <param name="questID">The ID of the quest to retrieve.</param>
        /// <returns>
        /// The matching QuestProgress instance, or null if no quest exists with the specified ID.
        /// </returns>
        public QuestProgress GetQuest(string questID)
        {
            QuestsDictionary.TryGetValue(
                questID,
                out QuestProgress quest);

            return quest;
        }

        /// <summary>
        /// Retrieves all quests currently in the specified state.
        /// </summary>
        /// <param name="questState">The quest state to filter by.</param>
        /// <returns>
        /// A list containing all quests that match the specified state.
        /// </returns>
        public List<QuestProgress> GetQuestsByState(QuestEnums.QuestState questState)
        {
            if(questState == QuestEnums.QuestState.Completed) return _activeQuests;
            
            return Quests
                .Where(q => q.QuestState == questState)
                .ToList();
        }
        
        #region Saving

        public object CaptureState()
        {
            QuestManagerSaveData saveData = new();

            foreach (QuestProgress quest in Quests)
            {
                QuestSaveData questSave = new()
                {
                    QuestID = quest.QuestInfo.QuestID,
                    QuestState = quest.QuestState
                };

                foreach (ObjectiveProgress objective in quest.Objectives)
                {
                    questSave.Objectives.Add(
                        new ObjectiveSaveData
                        {
                            ObjectiveID = objective.Objective.ObjectiveID,
                            CurrentAmount = objective.CurrentAmount
                        });
                }

                saveData.Quests.Add(questSave);
            }

            return saveData;
        }

        public void RestoreState(string state)
        {
            QuestManagerSaveData saveData = JsonUtility.FromJson<QuestManagerSaveData>(state);

            _activeQuests.Clear();

            foreach (QuestSaveData savedQuest in saveData.Quests)
            {
                QuestProgress quest =
                    GetQuest(savedQuest.QuestID);

                if (quest == null)
                    continue;

                quest.QuestState = savedQuest.QuestState;

                foreach (ObjectiveSaveData savedObjective in savedQuest.Objectives)
                {
                    ObjectiveProgress objective =
                        quest.Objectives.Find(
                            o => o.Objective.ObjectiveID ==
                                 savedObjective.ObjectiveID);

                    if (objective == null)
                        continue;

                    objective.CurrentAmount =
                        savedObjective.CurrentAmount;
                }

                if (quest.QuestState ==
                    QuestEnums.QuestState.Active)
                {
                    _activeQuests.Add(quest);
                }
            }
        }
        
        [Serializable]
        public class QuestManagerSaveData
        {
            public List<QuestSaveData> Quests = new();
        }

        [Serializable]
        public class QuestSaveData
        {
            public string QuestID;
            public QuestEnums.QuestState QuestState;

            public List<ObjectiveSaveData> Objectives = new();
        }

        [Serializable]
        public class ObjectiveSaveData
        {
            public string ObjectiveID;
            public int CurrentAmount;
        }
        #endregion

    }
}
