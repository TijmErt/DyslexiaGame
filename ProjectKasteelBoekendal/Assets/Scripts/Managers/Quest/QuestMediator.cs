using System;
using UnityEngine;

namespace Managers.Quest
{
    public class QuestMediator : MonoBehaviour
    {
        private QuestEnums.ObjectiveType _type;
        private string _targetID;
        
        public event Action OnQuestChanged; // Main usage is for other scripts to detect when the quest manager does something.

        #region ActionEvent

        private void OnEnable()
        {
            QuestManager.instance.OnQuestChanged += HandleFlagChanged;
        }

        private void OnDisable()
        {
            if (QuestManager.instance != null) 
                QuestManager.instance.OnQuestChanged -= HandleFlagChanged;
        }

        private void HandleFlagChanged()
        {
            OnQuestChanged?.Invoke();
        }

        #endregion

        public void SetTypeReachLocation()
        {
            _type = QuestEnums.ObjectiveType.ReachLocation;
        }
        public void SetTypeInteract()
        {
            _type = QuestEnums.ObjectiveType.Interact;
        }
        public void SetTypeAmount()
        {
            _type = QuestEnums.ObjectiveType.CompleteAmount;
        }

        public void SetTargetID(string targetID)
        {
            _targetID = targetID;
        }
        
        public void NotifyQuest(int amount)
        {
            QuestManager.instance.ReportProgress(
                _type,
                _targetID,
                amount);
        }
        
        public void NotifyQuest(QuestEnums.ObjectiveType type, string targetID, int amount)
        {
            QuestManager.instance.ReportProgress(
                type,
                targetID,
                amount);
        }
        
    }
}
