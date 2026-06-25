using System;

namespace Managers.Quest
{
    [Serializable]
    public class ObjectiveProgress
    {
        public ObjectiveInfo Objective;
        public int CurrentAmount;
        public bool IsCompleted => CheckCompletion();

        public ObjectiveProgress(ObjectiveInfo objective)
        {
            Objective = objective;
        }

        private bool CheckCompletion()
        {
            return Objective != null &&
                   CurrentAmount >= Objective.RequiredAmount;
        }

        public void AdvanceAmount(int amount)
        {
            if (Objective.RequiredAmount == 1 && ( Objective.Type == QuestEnums.ObjectiveType.ReachLocation || Objective.Type == QuestEnums.ObjectiveType.Interact  ))
            {
                CurrentAmount = 1;
            }
            else
            {
                CurrentAmount += amount;
            }

        }
    }
}
