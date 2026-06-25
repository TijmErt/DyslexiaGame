using UnityEngine;

namespace Managers.Quest
{
    public class QuestMediator : MonoBehaviour
    {
        private QuestEnums.ObjectiveType _type;
        private string _targetID;

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
