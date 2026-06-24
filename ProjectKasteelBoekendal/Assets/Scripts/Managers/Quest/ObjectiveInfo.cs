using System;
using UnityEngine;

namespace Managers.Quest
{
    [Serializable]
    public class ObjectiveInfo
    {
        public string ObjectiveID;
        [TextArea] public string Description;
        public QuestEnums.ObjectiveType Type;
        [Min(1)] public int RequiredAmount;
        public string TargetID;// The GameObject name or ID, used to determine if who to interact with and if an interaction has been done.
        public string SceneName; //Name of the scene the objective is found in

    }
}
