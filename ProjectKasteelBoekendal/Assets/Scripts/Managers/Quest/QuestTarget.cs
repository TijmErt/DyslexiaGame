using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers.Quest
{
    public class QuestTarget : MonoBehaviour
    {
        public string targetID;
        public bool isFocusOfQuest = false;
        
        public GameObject questMarker;
        public Vector3 questTargetLocation;
        
        private QuestMediator _questMediator;
        private void Awake()
        {
            _questMediator = FindFirstObjectByType<QuestMediator>();
            questTargetLocation = this.gameObject.transform.position;
            
            QuestTargetRegistry.Instance.Register(this);
        }

        private void OnDestroy()
        {
            QuestTargetRegistry.Instance.Unregister(this);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (isFocusOfQuest && other.CompareTag("Player1"))
            {
                _questMediator.NotifyQuest(QuestEnums.ObjectiveType.ReachLocation,targetID,1);
            }
        }

        public Vector3 FocusQuestTarget( )
        {
            isFocusOfQuest = true;

            if (questMarker != null)
                questMarker.SetActive(true);
            
            return questTargetLocation; // We return a Vector so that it can be used for a pointer element in the UI that points towards this QuestTarget
        }

        public void UnFocusQuestTarget()
        {
            isFocusOfQuest = false;
     
            if (questMarker != null)
                questMarker.SetActive(false);
        }
    }
}
