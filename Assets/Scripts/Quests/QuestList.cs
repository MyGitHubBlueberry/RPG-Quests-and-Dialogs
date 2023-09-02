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

      private bool HasQuest(Quest quest)
      {
         foreach (QuestStatus status in questStatuses)
         {
            if(status.GetQuest() == quest) return true;
         }
         return false;
      }
   }
}
