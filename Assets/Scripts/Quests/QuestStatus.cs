using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        [SerializeField] Quest quest;
        [SerializeField] List<string> completedObjectives = new List<string>();

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public Quest GetQuest() => quest;
        public int GetCompletedObjectivesCount() => completedObjectives.Count;
        public bool IsObjectiveCompleted(string objective) => completedObjectives.Contains(objective);
        public void CompleteObjective(string objective) => completedObjectives.Add(objective);
    }
}
