using System;
using System.Collections.Generic;
using System.Linq;
using Managers.Saving;
using UnityEngine;

namespace Managers.Quest
{
    public class QuestManager : MonoBehaviour, ISaveable
    {
        public static QuestManager instance;
        public string UID => "QuestManager";

        public List<QuestProgress> Quests = new List<QuestProgress>();
        private Dictionary<string, QuestProgress> QuestsDictionary = new Dictionary<string, QuestProgress>();
        
        public List<QuestProgress> _activeQuests = new();

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
        
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Initialize();
        }

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

        private void CheckAvailability()
        {
            foreach (QuestProgress quest in Quests)
            {
                if (quest.QuestState != QuestEnums.QuestState.Inactive)
                    continue;

                if (string.IsNullOrEmpty(
                        quest.QuestInfo.RequiredEventFlag))
                {
                    ActivateQuest(quest.QuestInfo.QuestID);
                    continue;
                }

                if (EventFlagManager.instance
                    .IsFlagEnabled(quest.QuestInfo.RequiredEventFlag))
                {
                    ActivateQuest(quest.QuestInfo.QuestID);
                }
            }
        }

        private void CompleteQuest(string questID)
        {
            if (!QuestsDictionary.TryGetValue(questID, out QuestProgress quest))
                return;

            if (quest.QuestState == QuestEnums.QuestState.Completed)
                return;

            quest.QuestState = QuestEnums.QuestState.Completed;
            
            if (!string.IsNullOrEmpty(quest.QuestInfo.CompletionEventFlag))
            {
                EventFlagManager.instance.ChangeFLagState(
                    quest.QuestInfo.CompletionEventFlag,true);
            }


        }

        private void ActivateQuest(string questID)
        {
            if (!QuestsDictionary.TryGetValue(questID, out QuestProgress quest))
                return;

            if (quest.QuestState != QuestEnums.QuestState.Inactive)
                return;

            quest.QuestState = QuestEnums.QuestState.Active;
            _activeQuests.Add(quest);
        }

        public void ReportProgress(QuestEnums.ObjectiveType objectiveType, string targetID, int amount)
        {
            List<QuestProgress> completedQuests = new();
            foreach (QuestProgress quest in _activeQuests)
            {
                quest.TryAdvanceObjective(
                    objectiveType,
                    targetID,
                    amount);

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

        public QuestProgress GetQuest(string questID)
        {
            QuestsDictionary.TryGetValue(
                questID,
                out QuestProgress quest);

            return quest;
        }

        public List<QuestProgress> GetQuestsByState(QuestEnums.QuestState questState)
        {
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
