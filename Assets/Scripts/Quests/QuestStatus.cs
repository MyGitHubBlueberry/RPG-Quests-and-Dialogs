using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [Serializable]
    public class QuestStatus
    {
        [SerializeField] Quest quest;
        [SerializeField] List<string> completedObjectives = new List<string>();

        public Quest GetQuest() => quest;
        public int GetCompletedObjectivesCount() => completedObjectives.Count;
        public bool IsObjectiveCompleted(string objective) => completedObjectives.Contains(objective);
    }
}
