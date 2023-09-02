using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
   public class QuestList : MonoBehaviour
   {
      List<QuestStatus> questStatuses = new List<QuestStatus>();

      public event Action OnQuestListUpdated;

      public IEnumerable<QuestStatus> GetStatuses() => questStatuses;
      public void AddQuest(Quest quest)
      {
         if (HasQuest(quest)) return;
         QuestStatus newStatus = new QuestStatus(quest);
         questStatuses.Add(newStatus);
         OnQuestListUpdated?.Invoke();
      }

      public bool HasQuest(Quest quest)
      {
         return GetQuestStatus(quest) != null;
      }

      public void CompleteObjective(Quest quest, string objective)
      {
         QuestStatus status = GetQuestStatus(quest);
         status.CompleteObjective(objective);
         OnQuestListUpdated?.Invoke();
      }
      
      private QuestStatus GetQuestStatus(Quest quest)
      {
         foreach (QuestStatus status in questStatuses)
         {
            if (status.GetQuest() == quest) return status;
         }
         return null;
      }
   }
}
