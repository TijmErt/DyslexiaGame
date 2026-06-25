using System;
using System.Collections.Generic;
using System.Linq;

namespace Managers.Quest
{
    /// <summary>
    /// Stores the runtime progress of a quest, including its current state
    /// and the progress of all associated objectives.
    /// </summary>
    [Serializable]
    public class QuestProgress
    {
        public QuestInfoSO QuestInfo;
        public QuestEnums.QuestState QuestState = QuestEnums.QuestState.Inactive;
        public List<ObjectiveProgress> Objectives = new List<ObjectiveProgress>();

        /// <summary>
        /// Creates a new quest progress instance and initializes progress tracking
        /// for each objective defined in the quest.
        /// </summary>
        /// <param name="questInfo">The quest definition to track progress for.</param>
        public QuestProgress(QuestInfoSO questInfo)
        {
            QuestInfo = questInfo;
            
            foreach (ObjectiveInfo objective in questInfo.objectives)
            {
                Objectives.Add(new ObjectiveProgress(objective));
            }
        }

        /// <summary>
        /// Attempts to advance objectives that match the provided type and target.
        /// Matching objectives will have their progress increased by the specified amount.
        /// </summary>
        /// <param name="objectiveType">The type of objective being progressed.</param>
        /// <param name="targetID">The identifier of the target associated with the objective.</param>
        /// <param name="amount">The amount of progress to add.</param>
        public bool TryAdvanceObjective(QuestEnums.ObjectiveType objectiveType, string targetID, int amount)
        {
            bool changes = false;
            foreach (ObjectiveProgress objective in Objectives)
            {
                if (objective.Objective.Type != objectiveType)
                    continue;

                if (objective.Objective.TargetID != targetID)
                    continue;

                objective.AdvanceAmount(amount);
                changes = true;
            }
            return changes;
        }
        
        /// <summary>
        /// Checks whether all objectives belonging to this quest have been completed.
        /// </summary>
        /// <returns>
        /// True if every objective is completed; otherwise false.
        /// </returns>
        public bool CheckCompletion()
        {
            bool completed;
            if(Objectives == null || Objectives.Count == 0)  completed = true;
            else completed =Objectives.All(objective => objective.IsCompleted);
            return completed;
        }
    }
}
