using System;
using System.Collections.Generic;
using System.Linq;

namespace Managers.Quest
{
    [Serializable]
    public class QuestProgress
    {
        public QuestInfoSO QuestInfo;
        public QuestEnums.QuestState QuestState = QuestEnums.QuestState.Inactive;
        public List<ObjectiveProgress> Objectives = new List<ObjectiveProgress>();

        public QuestProgress(QuestInfoSO questInfo)
        {
            QuestInfo = questInfo;
            
            foreach (ObjectiveInfo objective in questInfo.objectives)
            {
                Objectives.Add(new ObjectiveProgress(objective));
            }
        }

        public void TryAdvanceObjective(QuestEnums.ObjectiveType objectiveType, string targetID, int amount)
        {
            foreach (ObjectiveProgress objective in Objectives)
            {
                if (objective.Objective.Type != objectiveType)
                    continue;

                if (objective.Objective.TargetID != targetID)
                    continue;

                objective.AdvanceAmount(amount);
            }
        }
        
        public bool CheckCompletion()
        {
            return Objectives.All(objective => objective.IsCompleted);
        }
    }
}
