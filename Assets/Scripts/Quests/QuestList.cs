using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Quests
{
   public class QuestList : MonoBehaviour, ISaveable
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
         if (status.IsCompleted()) GiveReward(quest);
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
      
      private void GiveReward(Quest quest)
      {
         foreach(var reward in quest.GetRewards())
         {
            bool isRewardAdded = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
            if(!isRewardAdded)
            {
               GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
            }
         }
      }

      public object CaptureState()
      {
         List<object> state = new List<object>();
         foreach (QuestStatus status in questStatuses)
         {
            state.Add(status.CaptureState());
         }
         return state;
      }

      public void RestoreState(object state)
      {
         List<object> stateList = state as List<object>;
         if (stateList == null) return;

         questStatuses.Clear();
         foreach (object objectState in stateList)
         {
            questStatuses.Add(new QuestStatus(objectState));
         }
      }
   }
}
