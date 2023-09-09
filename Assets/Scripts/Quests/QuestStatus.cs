using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        Quest quest;
        List<string> completedObjectives = new List<string>();

        [Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives;
            public QuestStatusRecord(Quest quest, List<string> completedObjectives)
            {
                questName = quest.name;
                this.completedObjectives = new List<string>(completedObjectives);
            }
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord questStatusRecord = objectState as QuestStatusRecord;
            if(questStatusRecord == null) return;

            quest = Quest.GetByName(questStatusRecord.questName);
            completedObjectives = questStatusRecord.completedObjectives;
        }

        public Quest GetQuest() => quest;
        public int GetCompletedObjectivesCount() => completedObjectives.Count;
        public bool IsObjectiveCompleted(string objective) => completedObjectives.Contains(objective);
        public void CompleteObjective(string objective) => completedObjectives.Add(objective);

        public object CaptureState()
        {
            return new QuestStatusRecord(quest, completedObjectives);
        }

        public bool IsCompleted()
        {
            foreach(var objective in quest.GetObjectives())
                if(!completedObjectives.Contains(objective.reference)) 
                    return false; 

            return true;
        }
    }
}
