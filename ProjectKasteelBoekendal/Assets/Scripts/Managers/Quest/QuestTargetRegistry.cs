using System.Collections.Generic;
using UnityEngine;

namespace Managers.Quest
{
    public class QuestTargetRegistry : MonoBehaviour
    {
        public static QuestTargetRegistry Instance;

        private Dictionary<string, QuestTarget> targets = new();

        private void Awake()
        {
            Instance = this;
        }

        public void Register(QuestTarget target)
        {
            if (!targets.ContainsKey(target.targetID))
            {
                targets.Add(target.targetID, target); 
                Debug.Log( "Registered Target: " + target.targetID);
            }
        }

        public void Unregister(QuestTarget target)
        {
            if (targets.ContainsKey(target.targetID))
            {
                targets.Remove(target.targetID); 
                Debug.Log( "UnRegistered Target: " + target.targetID);
            }
                
        }

        public QuestTarget Get(string id)
        {
            targets.TryGetValue(id, out var target);
            return target;
        }
    }
}