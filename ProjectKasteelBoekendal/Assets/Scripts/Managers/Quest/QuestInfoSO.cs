using System.Collections.Generic;
using UnityEngine;

namespace Managers.Quest
{
    [CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/QuestInfoSO")]
    public class QuestInfoSO : ScriptableObject
    {
        public string QuestID;
        public string QuestName;
        public string CompletionEventFlag;
        public string RequiredEventFlag;
        public List<ObjectiveInfo> objectives = new List<ObjectiveInfo>();
    }
}
