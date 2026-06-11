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
        
        private List<QuestInfoSO> _allQuests = new List<QuestInfoSO>();
        
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
            _allQuests =
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
            throw new System.NotImplementedException();
        }

        public void RestoreState(string state)
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }
}
